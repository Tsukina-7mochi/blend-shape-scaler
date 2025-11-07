using UnityEngine;

namespace net.ts7m.shape_key_scaler.editor {
    public class BlendShape {
        public string Name;
        public (float, BlendShapeFrame)[] Frames;

        public float MaxWeight => this.Frames[^1].Item1;

        public BlendShape(string name, (float, BlendShapeFrame)[] frames) {
            this.Name = name;
            this.Frames = frames;
        }

        /**
         * Calculates BlendShapeFrame at the given weight, linearly interpolating between
         * the neighbouring frames. If the weight is less than 0 or greater than the maximum frame
         * weight, a frame scaled with the first or last frame is returned.
         */
        public BlendShapeFrame LerpFrameOn(float weight) {
            var firstWeight = this.Frames[0].Item1;

            if (this.Frames.Length <= 1 || weight <= firstWeight) {
                // simply scale to targeted weight with first frame
                return (weight / firstWeight) * this.Frames[0].Item2;
            }

            // find a frame with weight that exceeds target weight
            // if target weight is grater than all frames weights, choose last frame
            var frameIndex = 1;
            for (; frameIndex < this.Frames.Length - 1; frameIndex++) {
                if (weight < this.Frames[frameIndex].Item1) break;
            }

            var (weight1, frame1) = this.Frames[frameIndex - 1];
            var (weight2, frame2) = this.Frames[frameIndex];
            var t = (weight - weight1) / (weight2 - weight1);
            return t * frame1 + (1 - t) * frame2;
        }
    }

    public static class MeshExtension {
        /** Gets a BlendShape frame as BlendShapeFrame at `frameIndex` in BlendShape at `shapeIndex`. */
        private static BlendShapeFrame _getBlendShapeFrame(
            this Mesh mesh,
            int shapeIndex,
            int frameIndex
        ) {
            var deltaVertices = new Vector3[mesh.vertexCount];
            var deltaNormals = new Vector3[mesh.vertexCount];
            var deltaTangents = new Vector3[mesh.vertexCount];
            mesh.GetBlendShapeFrameVertices(
                shapeIndex,
                frameIndex,
                deltaVertices,
                deltaNormals,
                deltaTangents
            );

            return new BlendShapeFrame(
                deltaVertices,
                deltaNormals,
                deltaTangents
            );
        }

        /** Gets all BlendShape frames in BlendShape at `shapeIndex`. */
        private static (float, BlendShapeFrame)[] _getBlendShapeFrames(
            this Mesh mesh,
            int shapeIndex
        ) {
            var numFrames = mesh.GetBlendShapeFrameCount(shapeIndex);

            var frames = new (float, BlendShapeFrame)[numFrames];
            for (var i = 0; i < numFrames; i++) {
                var weight = mesh.GetBlendShapeFrameWeight(shapeIndex, i);
                frames[i] = (weight, mesh._getBlendShapeFrame(shapeIndex, i));
            }

            return frames;
        }

        /** Gets a BlendShape at the `shapeIndex`. */
        public static BlendShape GetBlendShape(this Mesh mesh, int shapeIndex) {
            var name = mesh.GetBlendShapeName(shapeIndex);
            var frames = mesh._getBlendShapeFrames(shapeIndex);
            return new BlendShape(name, frames);
        }

        /** * Add BlendShapes to this mesh. */
        public static void AddBlendShapeFrames(this Mesh mesh, BlendShape blendShape) {
            foreach (var (weight, frame) in blendShape.Frames) {
                mesh.AddBlendShapeFrame(
                    blendShape.Name,
                    weight,
                    frame.DeltaVertices,
                    frame.DeltaNormals,
                    frame.DeltaTangents
                );
            }
        }

        public static void CopyBlendShapeFramesFrom(
            this Mesh mesh,
            Mesh fromMesh,
            int sourceShapeIndex,
            string destinationShapeName
        ) {
            var numFrames = fromMesh.GetBlendShapeFrameCount(sourceShapeIndex);

            for (var i = 0; i < numFrames; i++) {
                var weight = fromMesh.GetBlendShapeFrameWeight(sourceShapeIndex, i);
                var frame = fromMesh._getBlendShapeFrame(sourceShapeIndex, i);
                mesh.AddBlendShapeFrame(
                    destinationShapeName,
                    weight,
                    frame.DeltaVertices,
                    frame.DeltaNormals,
                    frame.DeltaTangents
                );
            }
        }
    }
}
