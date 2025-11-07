using UnityEngine;

namespace net.ts7m.shape_key_scaler.editor {
    public class BlendShapeFrame {
        public readonly Vector3[] DeltaVertices;
        public readonly Vector3[] DeltaNormals;
        public readonly Vector3[] DeltaTangents;

        public BlendShapeFrame(
            Vector3[] deltaVertices,
            Vector3[] deltaNormals,
            Vector3[] deltaTangents
        ) {
            this.DeltaVertices = deltaVertices;
            this.DeltaNormals = deltaNormals;
            this.DeltaTangents = deltaTangents;
        }

        /** Adds all deltaVertices, deltaNormals and deltaTangents. */
        public static BlendShapeFrame operator +(BlendShapeFrame f1, BlendShapeFrame f2) {
            var deltaVertices = new Vector3[f1.DeltaVertices.Length];
            Vector3[] deltaNormals = null;
            Vector3[] deltaTangents = null;

            for (var i = 0; i < f1.DeltaVertices.Length; i++) {
                deltaVertices[i] = f1.DeltaVertices[i] + f2.DeltaVertices[i];
            }

            if (f1.DeltaNormals != null) {
                deltaNormals = new Vector3[f1.DeltaNormals.Length];
                for (var i = 0; i < f1.DeltaVertices.Length; i++) {
                    deltaNormals[i] = f1.DeltaNormals[i] + f2.DeltaNormals[i];
                }
            }

            if (f1.DeltaTangents != null) {
                deltaTangents = new Vector3[f1.DeltaTangents.Length];
                for (var i = 0; i < f1.DeltaVertices.Length; i++) {
                    deltaTangents[i] = f1.DeltaTangents[i] + f2.DeltaTangents[i];
                }
            }

            return new BlendShapeFrame(deltaVertices, deltaNormals, deltaTangents);
        }

        /** Multiplies the given scale to all deltaVertices, deltaNormals and deltaTangents. */
        public static BlendShapeFrame operator *(BlendShapeFrame frame, float scale) {
            var deltaVertices = new Vector3[frame.DeltaVertices.Length];
            Vector3[] deltaNormals = null;
            Vector3[] deltaTangents = null;

            for (var i = 0; i < frame.DeltaVertices.Length; i++) {
                deltaVertices[i] = frame.DeltaVertices[i] * scale;
            }

            if (frame.DeltaNormals != null) {
                deltaNormals = new Vector3[frame.DeltaNormals.Length];
                for (var i = 0; i < frame.DeltaVertices.Length; i++) {
                    deltaNormals[i] = frame.DeltaNormals[i] * scale;
                }
            }

            if (frame.DeltaTangents != null) {
                deltaTangents = new Vector3[frame.DeltaTangents.Length];
                for (var i = 0; i < frame.DeltaVertices.Length; i++) {
                    deltaTangents[i] = frame.DeltaTangents[i] * scale;
                }
            }

            return new BlendShapeFrame(deltaVertices, deltaNormals, deltaTangents);
        }

        /** Multiplies the given scale to all deltaVertices, deltaNormals and deltaTangents. */
        public static BlendShapeFrame operator *(float scale, BlendShapeFrame frame) {
            return frame * scale;
        }

        /** Subtracts all deltaVertices, deltaNormals and deltaTangents of `f2` from `f1`. */
        public static BlendShapeFrame operator -(BlendShapeFrame f1, BlendShapeFrame f2) {
            return f1 + (-1 * f2);
        }
    }
}
