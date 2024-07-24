module BaseNoise

open FastNoiseLite
open Config

let createBaseNoise =
    let continentNoise = FastNoiseLite(SEED)
    continentNoise.SetNoiseType(NoiseType.Cellular)
    continentNoise.SetCellularDistanceFunction(CellularDistanceFunction.Euclidean)
    continentNoise.SetCellularJitter(1.8f)
    continentNoise.SetFractalType(FractalType.Ridged)
    continentNoise.SetFrequency(CONTINENT_NOISE_FREQUENCY)

    let coastNoise = FastNoiseLite(SEED + 1)
    coastNoise.SetNoiseType(NoiseType.Perlin)
    coastNoise.SetFrequency(COAST_NOISE_FREQUENCY)


    let detailNoise = FastNoiseLite(SEED + 2)
    detailNoise.SetNoiseType(NoiseType.Perlin)
    detailNoise.SetFrequency(DETAIL_NOISE_FREQUENCY)

    let shapeNoise = FastNoiseLite(SEED + 3)
    shapeNoise.SetNoiseType(NoiseType.Perlin)
    shapeNoise.SetFrequency(SHAPE_NOISE_FREQUENCY)
    shapeNoise.SetDomainWarpType(DomainWarpType.OpenSimplex2)
    shapeNoise.SetFractalType(FractalType.Ridged)

    let mountainNoise = FastNoiseLite(SEED + 4)
    mountainNoise.SetNoiseType(NoiseType.Perlin)
    mountainNoise.SetFrequency(MOUNTAIN_NOISE_FREQUENCY)

    log Info "Generating base noise..."
    Array2D.init 
        CHUNK_SIZE 
        CHUNK_SIZE 
        (fun x y ->
            let x0 = float32 (x + X_OFFSET)
            let y0 = float32 (y + Y_OFFSET)
            let nx = x0 / (ZOOM * float32 CHUNK_SIZE)
            let ny = y0 / (ZOOM * float32 CHUNK_SIZE)
            
            let continentValue = continentNoise.GetNoise(nx, ny)
            let coastValue = coastNoise.GetNoise(nx, ny) * COAST_NOISE_AMPLITUDE
            let detailValue = detailNoise.GetNoise(nx, ny) * DETAIL_NOISE_AMPLITUDE
            let shapeValue = shapeNoise.GetNoise(nx, ny)
            let mountainValue = mountainNoise.GetNoise(nx, ny) * MOUNTAIN_NOISE_AMPLITUDE
            
            let combinedNoise = continentValue + coastValue + detailValue
            
            // Use shapeValue to modulate the land/ocean threshold
            let adjustedThreshold = LAND_THRESHOLD + (shapeValue * SHAPE_INFLUENCE)
            
            if combinedNoise > adjustedThreshold then
                combinedNoise + (abs mountainValue)  // Add mountain elevation to land
            else
                (combinedNoise - OCEAN_DEPTH_OFFSET) * OCEAN_DEPTH_FACTOR + OCEAN_DEPTH_OFFSET
        )