module ImageGen

open BaseNoise
open Config

open System.Drawing
open System.Runtime.Serialization.Formatters.Binary
open System.Drawing.Imaging
open System.Runtime.InteropServices
open BlockMapping

let createPngFromColorArray (filePath: string) (colors: Color[,])  =
    let width = colors.GetLength(0)
    let height = colors.GetLength(1)
    
    use bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb)
    let bitmapData = bitmap.LockBits(
        Rectangle(0, 0, width, height),
        ImageLockMode.WriteOnly,
        PixelFormat.Format32bppArgb)
    
    let ptr = bitmapData.Scan0
    let bytes = Array.zeroCreate<byte> (width * height * 4)
    let mutable i = 0
    
    for y in 0 .. height - 1 do
        for x in 0 .. width - 1 do
            let color = colors.[x, y]
            bytes.[i] <- color.B
            bytes.[i + 1] <- color.G
            bytes.[i + 2] <- color.R
            bytes.[i + 3] <- color.A
            i <- i + 4
    
    Marshal.Copy(bytes, 0, ptr, bytes.Length)
    bitmap.UnlockBits(bitmapData)
    
    bitmap.Save(filePath, ImageFormat.Png)

let toColors (a: BlockInfo )=
    match a.BlockType with
    | Snow -> Color.White
    | Rock -> Color.SlateGray
    | Shrub -> Color.SpringGreen
    | Tree -> Color.ForestGreen
    | Grass -> Color.DarkSeaGreen
    | Sand -> Color.Tan
    | Water -> Color.RoyalBlue
    | DeepWater -> Color.DarkBlue

let writeImageOfMapToFile values = 
    log Info "Writing Image"
    Array2D.map toColors values
    |> createPngFromColorArray "./fs_map.png"