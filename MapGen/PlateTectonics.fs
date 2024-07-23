module PlateTectonics

open Stride.Core.Mathematics
open System
open Config


type Plate = { Position: Vector2; Velocity: Vector2 }

let private velocityFunction x = x * 2f - 1f

let private createPlates numPlates size =
    [| for _ in 1..numPlates ->
        { Position = Vector2(rand.NextSingle() * float32 size, rand.NextSingle() * float32 size)
          Velocity = Vector2(rand.NextSingle() |> velocityFunction, rand.NextSingle()|> velocityFunction) }
    |]

let private movePlates (plates: Plate array) =
    plates |> Array.map (fun plate ->
        { plate with Position = plate.Position + plate.Velocity }
    )

let private simulatePlates iterations size =
    let mutable plates = createPlates TECTONIC_PLATE_COUNT size
    for _ in 1..iterations do
        plates <- movePlates plates
    plates

let private createPlateNoise size = 
    let plates = simulatePlates TECTONIC_PLATE_ITERATIONS size
    Array2D.init size size (fun x y ->
        let pos = Vector2(float32 x, float32 y)
        plates 
        |> Array.map (fun plate -> Vector2.Distance(pos, plate.Position))
        |> Array.min
    )