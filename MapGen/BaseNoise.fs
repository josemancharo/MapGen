module BaseNoise

open FastNoiseLite
open Config

let createBaseNoise =
    let continentNoise = FastNoiseLite(SEED)
    continentNoise.SetNoiseType(NoiseType.Cellular)
    continentNoise.SetCellularDistanceFunction(CellularDistanceFunction.Euclidean)
    continentNoise.SetCellularJitter(1.2f)
    continentNoise.SetFractalType(FractalType.FBm)
    continentNoise.SetFrequency(CONTINENT_NOISE_FREQUENCY)

    let continentDetailNoise = FastNoiseLite(SEED + 1)
    continentDetailNoise.SetNoiseType(NoiseType.Perlin)
    continentDetailNoise.SetFrequency(CONTINENT_DETAIL_FREQUENCY)

    let coastNoise = FastNoiseLite(SEED + 2)
    coastNoise.SetNoiseType(NoiseType.Perlin)
    coastNoise.SetFrequency(COAST_NOISE_FREQUENCY)

    let mountainNoise = FastNoiseLite(SEED + 3)
    mountainNoise.SetNoiseType(NoiseType.Perlin)
    mountainNoise.SetFractalType(FractalType.Ridged)
    mountainNoise.SetFrequency(MOUNTAIN_NOISE_FREQUENCY)

    let riverNoise = FastNoiseLite(SEED + 4)
    riverNoise.SetNoiseType(NoiseType.Perlin)
    riverNoise.SetFrequency(RIVER_NOISE_FREQUENCY)

    let warpNoise = FastNoiseLite(SEED + 5)
    warpNoise.SetDomainWarpType(DomainWarpType.OpenSimplex2)
    warpNoise.SetFrequency(WARP_FREQUENCY)
    warpNoise.SetDomainWarpAmp(WARP_AMPLITUDE)

    log Info "Generating base noise..."
    Array2D.init 
        CHUNK_SIZE 
        CHUNK_SIZE 
        (fun x y ->
            let x0 = float32 (x + X_OFFSET)
            let y0 = float32 (y + Y_OFFSET)
            let nx = x0 / (ZOOM * float32 CHUNK_SIZE)
            let ny = y0 / (ZOOM * float32 CHUNK_SIZE)
            
            // Apply domain warping
            let mutable wx = nx
            let mutable wy = ny
            warpNoise.DomainWarp(&wx, &wy)
            
            let continentValue = continentNoise.GetNoise(wx, wy)
            let continentDetailValue = continentDetailNoise.GetNoise(wx, wy) * CONTINENT_DETAIL_AMPLITUDE
            let coastValue = coastNoise.GetNoise(wx, wy) * COAST_NOISE_AMPLITUDE
            let mountainValue = mountainNoise.GetNoise(wx, wy) * MOUNTAIN_NOISE_AMPLITUDE
            let riverValue = riverNoise.GetNoise(wx, wy) * RIVER_NOISE_AMPLITUDE
            
            let combinedNoise = continentValue + continentDetailValue + coastValue
            
            // Adjust the terracing function
            let terracedNoise = 
                if combinedNoise > LAND_THRESHOLD then
                    let t = (combinedNoise - LAND_THRESHOLD) / (1.0f - LAND_THRESHOLD)
                    LAND_THRESHOLD + (floor (t * TERRACE_STEPS) / TERRACE_STEPS) * (1.0f - LAND_THRESHOLD)
                else
                    combinedNoise
            
            // Add mountains and rivers
            let finalNoise =
                if terracedNoise > LAND_THRESHOLD then
                    terracedNoise + (abs mountainValue) - (abs riverValue)
                else
                    (terracedNoise - OCEAN_DEPTH_OFFSET) * OCEAN_DEPTH_FACTOR + OCEAN_DEPTH_OFFSET
            
            finalNoise
        )