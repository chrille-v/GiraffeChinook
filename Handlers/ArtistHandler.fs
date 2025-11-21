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
                    |> Seq.toList

                return! json artistQuery next ctx
            }

    let getAllArtistsWithAlbumsHttpHandler (db: ChinookContext) : HttpHandler =
        fun next ctx ->
            task {
                let artists =
                    query {
                        for artist in db.Artist do
                        join albums in db.Album on (artist.ArtistId = albums.ArtistId)
                        select (artist, albums)
                    }
                    |> Seq.toList

                let result =
                    artists
                    |> List.groupBy (fun (x, _) -> x)
                    |> List.map (fun (artist, pairs) ->
                        {
                            ArtistId = artist.ArtistId
                            Name = artist.Name
                            Album =
                                pairs
                                |> List.map (fun (_, album) ->
                                    {
                                        AlbumId = album.AlbumId
                                        Title = album.Title
                                        ArtistId = album.ArtistId
                                    })
                        })
                
                return! json result next ctx
            }

    let getByIdHttpHandler (id: int) (db: ChinookContext) : HttpHandler = 
        fun next ctx  ->
            task {
                let result : ArtistRepository =
                    query {
                        for artist in db.Artist do
                        where (artist.ArtistId = id)
                        select artist
                        exactlyOneOrDefault
                }

                return! json result next ctx
            }

    let getArtistWithAlbumsHttpHandler (id: int) (db: ChinookContext) : HttpHandler =
        fun next ctx ->
            task {
                let artists  =
                    query {
                        for artist in db.Artist do
                        join albums in db.Album
                            on (artist.ArtistId = albums.ArtistId)
                        where (artist.ArtistId = id)
                        select (artist, albums)
                    }
                    |> Seq.toList

                let result =
                    artists
                    |> List.groupBy (fun (x, _) -> x)
                    |> List.map (fun (artist, pairs) ->
                        {
                            ArtistId = artist.ArtistId
                            Name = artist.Name
                            Album =
                                pairs
                                |> List.map (fun (_, album) ->
                                    {
                                        AlbumId = album.AlbumId
                                        Title = album.Title
                                        ArtistId = album.ArtistId
                                    })
                        })

                return! json result next ctx
            }