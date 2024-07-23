open Config
open BaseNoise
open PlateTectonics
open Erosion
open Biomes
open BlockMapping
open Chunking
open ImageGen


[<EntryPoint>]
let main args = 
    createBaseNoise SEED ZOOM X_OFFSET Y_OFFSET
    // |> generateErodedNoise
    |> generateBiomeMap
    |> getBlockInfoMap
    |> writeImageOfMapToFile
    log Info "Done!"
    0