using System;
using Algolia.Search.Models.Common;
using nadena.dev.ndmf;
using net.ts7m.shape_key_scaler.editor;
using UnityEngine;
using InvalidOperationException = System.InvalidOperationException;
using Object = UnityEngine.Object;

[assembly: ExportsPlugin(typeof(BlendShapeScalerProcessor))]

namespace net.ts7m.shape_key_scaler.editor {
    public class BlendShapeScalerProcessor : Plugin<BlendShapeScalerProcessor> {
        public override string QualifiedName => "net.ts7m.shape_key_scaler.BlendShapeScaler";
        public override string DisplayName => "BlendShape Scaler";

        protected override void Configure() {
            this.InPhase(BuildPhase.Transforming).Run(BlendShapeScalerPass.Instance);
        }
    }

    internal class BlendShapeScalerPass : Pass<BlendShapeScalerPass> {
        protected override void Execute(BuildContext ctx) {
            var components = ctx.AvatarRootTransform.GetComponentsInChildren<BlendShapeScaler>();
            foreach (var component in components) {
                BlendShapeScalerPass._processComponent(component, ctx);
                Object.DestroyImmediate(component);
            }
        }

        private static void _processComponent(BlendShapeScaler component, BuildContext ctx) {
            var renderer = component.GetSkinnedMeshRenderer();
            var mesh = renderer?.sharedMesh;
            if (mesh == null) {
                throw new InvalidOperationException("Cannot find Mesh in SkinnedMeshRenderer");
            }

            // copy and clear BlendShapes
            var newMesh = Object.Instantiate(mesh);
            newMesh.ClearBlendShapes();

            // verify all targets has correct name
            foreach (var target in component.Targets) {
                if (mesh.GetBlendShapeIndex(target.Name) < 0) {
                    throw new InvalidOperationException($"Cannot find BlendShape: {target.Name}");
                }
            }

            // Because BlendShape frames can only be added to the last BlendShape in the mesh,
            // iterate over all blend shapes modifying them to copy them to newMesh.
            for (var shapeIndex = 0; shapeIndex < mesh.blendShapeCount; ++shapeIndex) {
                var blendShapeName = mesh.GetBlendShapeName(shapeIndex);

                ScalerTarget target = null;
                foreach (var t in component.Targets) {
                    if (t.Name == blendShapeName) {
                        target = t;
                    }
                }

                if (target == null) {
                    // directly copy BlendShape
                    newMesh.CopyBlendShapeFramesFrom(mesh, shapeIndex, blendShapeName);
                    continue;
                }

                var blendShape = mesh.GetBlendShape(shapeIndex);
                var maxWeight = target.Scale * blendShape.MaxWeight;

                if (maxWeight < 1e-6) {
                    // only one frame at the target weight
                    blendShape.Frames = new[] {
                        (blendShape.MaxWeight, blendShape.LerpFrameOn(maxWeight))
                    };
                } else if (maxWeight >= blendShape.MaxWeight) {
                    // extend range of last frame and adjust other frame weights
                    var weightScale = blendShape.MaxWeight / maxWeight;
                    blendShape.Frames[^1] = (maxWeight, blendShape.LerpFrameOn(maxWeight));
                    for (var i = 0; i < blendShape.Frames.Length; i++) {
                        var (weight, frame) = blendShape.Frames[i];
                        blendShape.Frames[i] = (weight * weightScale, frame);
                    }
                } else {
                    // 0 < maxWeight < blendShape.MaxWeight
                    var frameIndex = 0;
                    for (; frameIndex < blendShape.Frames.Length - 1; frameIndex++) {
                        if (maxWeight < blendShape.Frames[frameIndex].Item1) break;
                    }

                    var newFrames = new (float, BlendShapeFrame)[frameIndex + 1];
                    for (var i = 0; i < frameIndex; i++) {
                        var (weight, frame) = blendShape.Frames[i];
                        newFrames[i] = (weight, frame);
                    }

                    newFrames[^1] = (maxWeight, blendShape.LerpFrameOn(maxWeight));

                    blendShape.Frames = newFrames;
                }

                newMesh.AddBlendShapeFrames(blendShape);
            }

            renderer.sharedMesh = newMesh;
        }
    }
}
