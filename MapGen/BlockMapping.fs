module BlockMapping

open BaseNoise
open Config
open Biomes

type BlockType =
    | DeepWater
    | Water
    | Sand
    | Grass
    | Tree
    | Shrub
    | Rock
    | Snow

type BlockInfo = 
  { BlockType: BlockType
    BiomeInfo: BiomeInfo
    IsWater: bool }
    
let (|LessThan|_|) k value = if value < k then Some() else None
let (|GToEQ|_|) k value = if value >= k then Some() else None


let private getBlock (info: BiomeInfo) =
    match info with 
    | { Elevation = GToEQ SNOW_LEVEL } -> 
        if info.Temperature >= SNOW_MAX_TEMPERATURE 
            then Rock
            else Snow

    | { Elevation = GToEQ MOUNTAIN_LEVEL } -> Rock
        
    | { Elevation = GToEQ SHRUB_LEVEL } -> 
        if info.BiomeType = Desert
            then Rock
            else Shrub

    | { Elevation = GToEQ TREE_LEVEL } ->
        match info.BiomeType with
        | Desert -> Sand
        | Plains -> Shrub
        | _ -> Tree

    | { Elevation = GToEQ GRASS_LEVEL } ->
        match info.BiomeType with
            | Jungle -> Tree
            | Desert -> Sand
            | _ -> Grass
    
    | { Elevation = GToEQ SAND_LEVEL } -> 
        if info.BiomeType = Desert
            then Grass
            else Sand
    
    | { Elevation = GToEQ WATER_LEVEL } -> Water
    | _ -> DeepWater

let private getBlockInfo (info: BiomeInfo) =
    let blockType = getBlock info
    { 
        BlockType = blockType
        BiomeInfo = info
        IsWater = blockType = Water || blockType = DeepWater
    }

let getBlockInfoMap biomeMap =
    biomeMap
    |> Array2D.map getBlockInfo