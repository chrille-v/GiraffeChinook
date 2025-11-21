namespace GiraffeChinook.Handlers

open System
open System.Collections.Generic
open Giraffe
open Microsoft.EntityFrameworkCore
open Microsoft.AspNetCore.Http
open FSharp.Linq
open GiraffeChinook.Data
open GiraffeChinook.Domain

module Album =
    let getAllAlbumsHttpHandler (db: ChinookContext) : HttpHandler =
        fun next ctx -> 
            task {
                let albums =
                    query {
                        for albums in db.Album do
                        select albums
                    }
                    |> Seq.toList
                
                return! json albums next ctx
            }