module Config

open System

let CHUNK_SIZE = 1024
let ZOOM = 0.25f
let SEED = 46584
let SCALE = 0.15f

let X_OFFSET = 0
let Y_OFFSET = 0

let DOMAIN_WARP_AMP = 10000f
let MASK_SCALE_OFFSET = 10f

let SNOW_LEVEL = 0.8f
let MOUNTAIN_LEVEL = 0.6f
let SHRUB_LEVEL = 0.5f
let TREE_LEVEL = 0.3f
let GRASS_LEVEL = 0.1f
let SAND_LEVEL = 0.0f
let WATER_LEVEL = -0.5f

let SNOW_MAX_TEMPERATURE = 0.8f
let DESERT_MAX_MOISTURE = 0.3f
let JUNGLE_MIN_MOISTURE = 0.8f
let TROPICAL_TEMP = 0.6f
let FOREST_MIN_TEMP = 0.4f
let TUNDRA_MAX_TEMP = 0.1f

let ADD_RIVERS = true
let RIVER_COUNT = 100
let APPLY_EROSION = true
let EROSION_ITERATIONS = 10
let TECTONIC_PLATE_ITERATIONS = 100
let TECTONIC_PLATE_COUNT = 10

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