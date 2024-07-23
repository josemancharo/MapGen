module BaseNoise

open FastNoiseLite
open System
open Stride.Core.Mathematics
open Config

let createBaseNoise =
    let continentNoise = FastNoiseLite(SEED)
    continentNoise.SetNoiseType(NoiseType.Perlin)
    continentNoise.SetFrequency(SCALE)

    //continentNoise.SetFractalType(FractalType.FBm)
    //continentNoise.SetFractalGain(0.005f)

    let detailNoise = FastNoiseLite(SEED + 1)
    detailNoise.SetNoiseType(NoiseType.Perlin)
    detailNoise.SetFrequency(SCALE * 10f)
    detailNoise.SetDomainWarpType(DomainWarpType.OpenSimplex2)
    detailNoise.SetDomainWarpAmp(DOMAIN_WARP_AMP)

    log Info "Generating base noise..."
    Array2D.init 
        CHUNK_SIZE 
        CHUNK_SIZE 
        (fun x y ->
            let x0 = x + X_OFFSET
            let y0 = y + Y_OFFSET
            let nx = float32 x0 / (ZOOM * float32 CHUNK_SIZE)
            let ny = float32 y0 / (ZOOM * float32 CHUNK_SIZE)
            let continentValue = continentNoise.GetNoise(nx, ny)
            let detailValue = detailNoise.GetNoise(nx, ny) * 0.2f
            continentValue + detailValue
        )