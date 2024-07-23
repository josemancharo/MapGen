module Erosion

open BaseNoise
open Config

let generateErodedNoise (heightMap: float32[,]) =
    log Info "Running erosion simulation..."
    let mutable erodedMap = Array2D.copy heightMap
    for _ in 1..EROSION_ITERATIONS do
        for x in 1..CHUNK_SIZE-2 do
            for y in 1..CHUNK_SIZE-2 do
                let neighbors = 
                    [|
                        erodedMap.[x-1,y]; erodedMap.[x+1,y]
                        erodedMap.[x,y-1]; erodedMap.[x,y+1]
                    |]
                let avgHeight = (Array.sum neighbors) / 4.0f
                erodedMap.[x,y] <- erodedMap.[x,y] * 0.99f + avgHeight * 0.01f
    erodedMap

