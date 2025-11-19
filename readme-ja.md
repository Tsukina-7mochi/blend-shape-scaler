# BlendShape Scaler

メッシュのブレンドシェイプ (シェイプキー) の上限値を変更する [NDMF](https://ndmf.nadena.dev/) プラグインです。このプラグインではブレンドシェイプのキーフレームの変更のみを行い、ブレンドシェイプの重み (`SkinnedMeshRenderer` で使用される 0 から 1 の値) は変更しません。複数のキーフレームを含むブレンドシェイプもサポートされています。

ユースケース:
- `SkinnedMeshRenderer` やブレンドシェイプアニメーションの設定を変更することなくブレンドシェイプの範囲を変更したい場合
- ブレンドシェイプアニメーションを正規化されたまま保ちながら、ブレンドシェイプ同士の干渉の影響を緩和させたい場合
  - 例えば、同じ頂点を動かすブレンドシェイプを同時に使用する際に変化量の合計が大きくならないようにスケールさせる
- ブレンドシェイプの上限値をより大きくしたい場合

## インストール

以下の VPM パッケージを VCC に追加してください。
詳しい追加方法は [VRChat のドキュメント](https://vcc.docs.vrchat.com/guides/community-repositories) を参照してください。

```
https://raw.githubusercontent.com/Tsukina-7mochi/blend-shape-scaler/refs/heads/main/vpm.json
```

その後、 `BlendShape Scaler` をプロジェクトに追加してください。

## 使用方法

`BlendShapeScaler` コンポーネントを `SkinnedMeshRenderer` の追加されているゲームオブジェクトに追加してください。その後上限値を変更するブレンドシェイプの名前とスケールを `Targets` フィールドに追加してください。
