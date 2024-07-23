open Config
open BaseNoise
open PlateTectonics
open Biomes
open BlockMapping
open Chunking
open ImageGen


[<EntryPoint>]
let main args = 
    createBaseNoise
    |> generateBiomeMap
    |> getBlockInfoMap
    |> writeImageOfMapToFile
    log Info "Done!"
    0