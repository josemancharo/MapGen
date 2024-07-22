module BaseNoise

open FastNoiseLite
open System
open Stride.Core.Mathematics

(*
    Good Seeds
    - 44765 (unicorn island)
*)

let SIZE = 2048
let ZOOM = 0.25f
let SEED = 44765
let SCALE = 1.2f
let X_OFFSET = -500
let Y_OFFSET = -500

let DOMAIN_WARP_AMP = 100.0f

let SNOW_THRESHOLD = 0.8f
let ROCK_THRESHOLD = 0.6f
let TREE_THRESHOLD = 0.5f
let SHRUB_THRESHOLD = 0.3f
let GRASS_THRESHOLD = 0.1f
let SAND_THRESHOLD = 0.0f
let WATER_THRESHOLD = -0.5f

let continentNoise = FastNoiseLite(SEED)
continentNoise.SetNoiseType(NoiseType.Perlin)
continentNoise.SetFrequency(SCALE)  // Much lower frequency for large-scale features

continentNoise.SetFractalType(FractalType.PingPong)

let detailNoise = FastNoiseLite(SEED + 1)
detailNoise.SetNoiseType(NoiseType.Perlin)
detailNoise.SetFrequency(SCALE * 10f)
detailNoise.SetDomainWarpType(DomainWarpType.OpenSimplex2)
detailNoise.SetDomainWarpAmp(DOMAIN_WARP_AMP)

type Mapping =
    | DeepWater
    | Water
    | Sand
    | Grass
    | Tree
    | Shrub
    | Rock
    | Snow

let toMapping (a: float32) =
    match a with
    | a when a >= SNOW_THRESHOLD -> Snow
    | a when a >= ROCK_THRESHOLD -> Rock
    | a when a >= TREE_THRESHOLD -> Tree
    | a when a >= SHRUB_THRESHOLD -> Shrub
    | a when a >= GRASS_THRESHOLD -> Grass
    | a when a >= SAND_THRESHOLD -> Sand
    | a when a >= WATER_THRESHOLD -> Water
    | _ -> DeepWater

let noiseValues: float32 array2d =
    printfn "Generating Noise..."
    Array2D.init 
        SIZE 
        SIZE 
        (fun x y ->
            let x0 = x + X_OFFSET
            let y0 = y + Y_OFFSET
            let nx = float32 x0 / (ZOOM * float32 SIZE)
            let ny = float32 y0 / (ZOOM * float32 SIZE)
            let continentValue = continentNoise.GetNoise(nx, ny)
            let detailValue = detailNoise.GetNoise(nx, ny) * 0.2f  // Reduced influence of detail
            continentValue + detailValue
        )