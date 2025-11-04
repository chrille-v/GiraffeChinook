namespace GiraffeChinook.Handlers

open System
open System.Collections.Generic
open Giraffe
open Microsoft.EntityFrameworkCore
open Microsoft.AspNetCore.Http
open FSharp.Linq
open GiraffeChinook.Data
open GiraffeChinook.Domain

module Artist = 
    let getAllHttpHandler (db: ChinookContext) : HttpHandler =
        fun next ctx -> 
            task {
                let artistQuery = 
                    query {
                        for artist in db.Artist do
                        select artist
                    }

                return! json artistQuery next ctx
            }

    let getByIdHttpHandler (id: int) (db: ChinookContext) : HttpHandler = 
        fun next ctx  ->
            task {
                let query : ArtistRepository =
                    query {
                        for artist in db.Artist do
                        where (artist.ArtistId = id)
                        select artist
                        exactlyOneOrDefault
                }

                return! json query next ctx
            }

    let getArtistWithAlbumsHttpHandler (id: int) (db: ChinookContext) : HttpHandler =
        fun next ctx ->
            task {
                let query  =
                    query {
                        for artist in db.Artist do
                        join albums in db.Album
                            on (artist.ArtistId = albums.ArtistId)
                        where (artist.ArtistId = id)
                        select (artist, albums)
                    }

                return! json query next ctx
            }