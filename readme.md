# BlendShape Scaler

A [NDMF](https://ndmf.nadena.dev/) plugin that scales BlendShape weights. This plugin can be used to extend or extend BlendShape weights. As this plugin only adds/removes BlendShape frames, the weights referenced by `SkinnedMeshRenderer` (0 to 1) remain unaffected. You can instantly extend or limit BlendShape weights without having to change any `SkinnedMeshRenderer` or animation files. BlendShapes with multiple frames are also supported.

## Installation

Add VPM package below to your VPM or VCC.
See [VRChat Document](https://vcc.docs.vrchat.com/guides/community-repositories) for the instruction.

```
https://raw.githubusercontent.com/Tsukina-7mochi/blend-shape-scaler/refs/heads/main/vpm.json
```

Then you can add `BlendShape Scaler` to your project.

## Usage

Attach `BlendShapeScaler` component to a object with `SkinnedMeshRenderer` you want to scale BlendShapes. Then add BlendShape names and scales to `Targets` field. When you enter to play mode, the scales will be applied.
