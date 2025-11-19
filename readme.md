# BlendShape Scaler

A [NDMF](https://ndmf.nadena.dev/) plugin that changes upper limit of BlendShapes. As this plugin only adds/removes BlendShape frames, the BlendShape weights referenced by `SkinnedMeshRenderer` (0 to 1) remain unaffected.BlendShapes with multiple frames are also supported.

Use cases:
- When you want to instantly extend or limit BlendShape weights without having to change any `SkinnedMeshRenderer` or animation files
- When you want to mitigate the effects of BlendShape interference while keeping BlendShape animation normalized
  - For example, when using BlendShapes that move the same vertex simultaneously, scale them so the total change amount does not become too large
- If you want to increase the upper limit of the blend shape


## Installation

Add VPM package below to your VPM or VCC.
See [VRChat Document](https://vcc.docs.vrchat.com/guides/community-repositories) for the instruction.

```
https://raw.githubusercontent.com/Tsukina-7mochi/blend-shape-scaler/refs/heads/main/vpm.json
```

Then you can add `BlendShape Scaler` to your project.

## Usage

Attach `BlendShapeScaler` component to a object with `SkinnedMeshRenderer` you want to scale BlendShapes. Then add BlendShape names and scales to `Targets` field.
