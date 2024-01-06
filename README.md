# LongArrayLib

サイズとインデックスを64 bit符号付き整数（`long`）で表すことのできる配列のライブラリです。

- ターゲット：.NET 7 & .NET 8
- 依存：なし
- NuGet：需要があるなら検討

## 特徴

- `LongArrayLib.LongArray<T>` が `long` 対応の配列
- `LongArrayLib.LongArray` に `LongArray<T>` を処理するメソッドが実装（`T[]` における `System.Array` に対応）
- `ICollection<T>` ， `T[]` にある
- LINQの拡張として `IEnumerable<T>` を `LongArray<T>` に変換する `ToLongArray()` を実装
- `Span<T>`, `Memory<T>`, `ReadOnlySequence<T>` との変換をサポート
- `Stream` で `LongArray<T>` を扱える拡張メソッドを実装
- C#12で追加されたコレクション式に対応

## `LongArray<T>` 実装

|       メソッド       |       実装クラス       |  対応する.NETの配列の実装   | 備考                                                     |
| :------------------: | :--------------------: | :-------------------------: | :------------------------------------------------------- |
|       `Create`       |      `LongArray`       |             N/A             | コレクションよりインスタンス生成                         |
|       `this[]`       |     `LongArray<T>`     |        `T[].this[]`         | `ref T` を返す。`Index` や `Range` のオーバーロードあり  |
|      `GetRange`      |     `LongArray<T>`     |             N/A             | `List<T>.GetRange()` と同じ取り回し                      |
|   `GetEnumerator`    |     `LongArray<T>`     |     `T[].GetEnumerator`     |                                                          |
| `GetSpanEnumerator`  |     `LongArray<T>`     |             N/A             | `Span<T>` に分割して列挙                                 |
|      `ForEach`       |     `LongArray<T>`     |          `ForEach`          |                                                          |
|    `ForEachChunk`    |     `LongArray<T>`     |             N/A             | `Span<T>` に分割して処理                                 |
|        `Copy`        |      `LongArray`       |        `Array.Copy`         | `T[]` と `LongArray<T>` 間の処理も実装                   |
|       `CopyTo`       |     `LongArray<T>`     |        `T[].CopyTo`         |                                                          |
|       `Clone`        |     `LongArray<T>`     |         `T[].Clone`         |                                                          |
|       `Resize`       |      `LongArray`       |       `Array.Resize`        |                                                          |
|       `Clear`        |      `LongArray`       |        `Array.Clear`        |                                                          |
|        `Fill`        |      `LongArray`       |        `Array.Fill`         |                                                          |
|      `Contains`      |     `LongArray<T>`     |      `Array.Contains`       |                                                          |
|      `IndexOf`       |     `LongArray<T>`     |       `Array.IndexOf`       |                                                          |
|    `LastIndexOf`     |     `LongArray<T>`     |     `Array.LastIndexOf`     |                                                          |
|       `Exists`       |     `LongArray<T>`     |       `Array.Exists`        |                                                          |
|     `TrueForAll`     |     `LongArray<T>`     |     `Array.TrueForAll`      |                                                          |
|        `Find`        |     `LongArray<T>`     |        `Array.Find`         |                                                          |
|      `FindLast`      |     `LongArray<T>`     |      `Array.FindLast`       |                                                          |
|     `FindIndex`      |     `LongArray<T>`     |      `Array.FindIndex`      |                                                          |
|   `FindLastIndex`    |     `LongArray<T>`     |    `Array.FindLastIndex`    |                                                          |
|    `BinarySearch`    |     `LongArray<T>`     |    `Array.BinarySearch`     | .NETと同様，見つからない場合はビット反転した値が挿入位置 |
|        `Sort`        |      `LongArray`       |        `Array.Sort`         | .NETと同様にイントロソート                               |
|      `Reverse`       |      `LongArray`       |       `Array.Reverse`       |                                                          |
|     `AsPointer`      |     `LongArray<T>`     |             N/A             |                                                          |
|      `ToArray`       |     `LongArray<T>`     |             N/A             | `T[]` に変換                                             |
|       `AsSpan`       | `LongMemoryExtensions` |  `MemoryExtensions.AsSpan`  |                                                          |
|      `AsMemory`      | `LongMemoryExtensions` | `MemoryExtensions.AsMemory` |                                                          |
| `AsReadOnlySequence` | `LongMemoryExtensions` |             N/A             | `ReadOnlySequence<T>` に変換                             |
