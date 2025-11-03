namespace GiraffeChinook.Handlers

open Giraffe
open Microsoft.EntityFrameworkCore
open Microsoft.AspNetCore.Http
open GiraffeChinook.Data
open GiraffeChinook.Domain

module Artist = 
    let getAllHttpHandler (db: ChinookContext) : HttpHandler =
        fun next ctx -> 
            task {
                let! (artists) = db.Artist.ToListAsync()

                return! json artists next ctx
            }