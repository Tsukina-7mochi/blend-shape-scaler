using System;
using UnityEngine;

namespace net.ts7m.shape_key_scaler {
    [Serializable]
    public class ScalerTarget {
        [SerializeField]
        private string _name;
        public string Name => this._name;

        [SerializeField]
        private float _scale;
        public float Scale => this._scale;
    }

    [AddComponentMenu("BlendShapeScaler/BlendShapeScaler")]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SkinnedMeshRenderer))]
    public class BlendShapeScaler : MonoBehaviour {
        [SerializeField]
        private ScalerTarget[] _targets;
        public ScalerTarget[] Targets => this._targets;

        public SkinnedMeshRenderer GetSkinnedMeshRenderer() {
            return this.GetComponent<SkinnedMeshRenderer>();
        }
    }
}
