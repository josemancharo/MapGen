module BaseNoise

open FastNoiseLite
open System
open Stride.Core.Mathematics
open Config

let createBaseNoise seed zoom x_offset y_offset =
    let continentNoise = FastNoiseLite(seed)
    continentNoise.SetNoiseType(NoiseType.Perlin)
    continentNoise.SetFrequency(SCALE)

    //continentNoise.SetFractalType(FractalType.FBm)
    //continentNoise.SetFractalGain(0.005f)

    let detailNoise = FastNoiseLite(seed + 1)
    detailNoise.SetNoiseType(NoiseType.Perlin)
    detailNoise.SetFrequency(SCALE * 10f)
    detailNoise.SetDomainWarpType(DomainWarpType.OpenSimplex2)
    detailNoise.SetDomainWarpAmp(DOMAIN_WARP_AMP)

    log Info "Generating base noise..."
    Array2D.init 
        CHUNK_SIZE 
        CHUNK_SIZE 
        (fun x y ->
            let x0 = x + x_offset
            let y0 = y + y_offset
            let nx = float32 x0 / (zoom * float32 CHUNK_SIZE)
            let ny = float32 y0 / (zoom * float32 CHUNK_SIZE)
            let continentValue = continentNoise.GetNoise(nx, ny)
            let detailValue = detailNoise.GetNoise(nx, ny) * 0.2f
            continentValue + detailValue
        )