module Config

open System

let CHUNK_SIZE = 1028
let ZOOM = 0.005f 
let SEED = 689794
let SCALE = 0.75f 

let X_OFFSET = 0
let Y_OFFSET = 0


// Noise Generation Parameters
let CONTINENT_NOISE_FREQUENCY = SCALE * 0.04f  // Slightly reduced to create larger continents
let COAST_NOISE_FREQUENCY = SCALE * 10f
let DETAIL_NOISE_FREQUENCY = SCALE * 10.0f
let SHAPE_NOISE_FREQUENCY = SCALE * 0.0025f
let MOUNTAIN_NOISE_FREQUENCY = SCALE * 0.05f

let COAST_NOISE_AMPLITUDE = 0.1f
let DETAIL_NOISE_AMPLITUDE = 0.02f
let MOUNTAIN_NOISE_AMPLITUDE = 0.5f

let LAND_THRESHOLD = -0.2f  // Slightly increased to reduce land area
let OCEAN_DEPTH_FACTOR = 0.4f // Smaller means more water
let OCEAN_DEPTH_OFFSET = -0.5f  // Lower to create deeper oceans

let SHAPE_INFLUENCE = 0.5f  // Slightly reduced to create more consistent landmasses

// Biome Parameters
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
    Fatal
    | Error
    | Warning
    | Info
    | Debug
    | Verbose

let log data level = 
    printfn "[%A] (%A) - %A" data DateTime.Now level