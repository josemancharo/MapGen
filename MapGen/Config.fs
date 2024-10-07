module Config

open System

let CHUNK_SIZE = 1028
let ZOOM = 0.005f  

let SEED = 689794
let SCALE = 1.0f

let X_OFFSET = 0
let Y_OFFSET = 0

// Noise Generation Parameters
let CONTINENT_NOISE_FREQUENCY = SCALE * 0.03f  // Decreased for larger continents
let CONTINENT_DETAIL_FREQUENCY = SCALE * 0.08f
let COAST_NOISE_FREQUENCY = SCALE * 0.4f
let MOUNTAIN_NOISE_FREQUENCY = SCALE * 0.2f
let RIVER_NOISE_FREQUENCY = SCALE * 0.1f
let WARP_FREQUENCY = SCALE * 0.004f

let CONTINENT_DETAIL_AMPLITUDE = 0.15f
let COAST_NOISE_AMPLITUDE = 0.08f
let MOUNTAIN_NOISE_AMPLITUDE = 1.5f
let RIVER_NOISE_AMPLITUDE = 0.05f
let WARP_AMPLITUDE = 25.0f

let LAND_THRESHOLD = -0.8f  // Lowered to increase land area
let OCEAN_DEPTH_FACTOR = 0.4f
let OCEAN_DEPTH_OFFSET = -0.3f  // Adjusted for better ocean depth variation

let TERRACE_STEPS = 8.0f  // Slightly reduced for smoother coastlines

// Biome Parameters (unchanged)
let SNOW_LEVEL = 0.8f
let MOUNTAIN_LEVEL = 0.6f
let SHRUB_LEVEL = 0.4f
let TREE_LEVEL = 0.2f
let GRASS_LEVEL = 0.1f
let SAND_LEVEL = -0.25f
let WATER_LEVEL = -0.35f

let SNOW_MAX_TEMPERATURE = 0.8f
let DESERT_MAX_MOISTURE = 0.3f
let JUNGLE_MIN_MOISTURE = 0.8f
let TROPICAL_TEMP = 0.6f
let FOREST_MIN_TEMP = 0.4f
let TUNDRA_MAX_TEMP = 0.1f

let rand = Random(SEED)

type LogLevel =
    | Fatal
    | Error
    | Warning
    | Info
    | Debug
    | Verbose

let log level data = 
    printfn "[%A] (%A) - %A" level DateTime.Now data