module Biomes

open FastNoiseLite
open BaseNoise
open Config

type BiomeType = 
    Plains 
    | Desert 
    | Forest 
    | Slopes 
    | Tundra 
    | Jungle

type BiomeInfo = 
  { BiomeType: BiomeType
    Elevation: float32
    Moisture: float32
    Temperature: float32 }

let moistureNoise = FastNoiseLite(SEED + 2)
moistureNoise.SetFrequency(SCALE*10f)

let temperatureNoise = FastNoiseLite(SEED + 3)
temperatureNoise.SetFrequency(SCALE*10f)

let private getBiome elevation moisture temperature =
    match elevation with
    | e when e > MOUNTAIN_LEVEL -> Slopes
    | _ -> 
        match (moisture, temperature) with
        | (m, t) when m < DESERT_MAX_MOISTURE && t > TROPICAL_TEMP -> Desert
        | (m, t) when m < DESERT_MAX_MOISTURE && t < TUNDRA_MAX_TEMP -> Tundra
        | (m, t) when m > JUNGLE_MIN_MOISTURE && t > TROPICAL_TEMP -> Jungle
        | (m, _) when m > FOREST_MIN_TEMP -> Forest
        | _ -> Plains

let private getBiomeInfo elevation moisture temperature =
    { 
        BiomeType = getBiome elevation moisture temperature
        Elevation = elevation
        Moisture = moisture
        Temperature = temperature
    }


let generateBiomeMap maskedNoise = 
    log Info "Generating biome information..."
    maskedNoise
    |> Array2D.mapi (fun x y elevation ->
        let moisture = (moistureNoise.GetNoise(float32 x, float32 y) + 1.0f) / 2.0f
        let temperature = (temperatureNoise.GetNoise(float32 x, float32 y) + 1.0f) / 2.0f
        getBiomeInfo elevation moisture temperature
    )