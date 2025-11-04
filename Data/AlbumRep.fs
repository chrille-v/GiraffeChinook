namespace GiraffeChinook.Data

open System.ComponentModel.DataAnnotations
open System.Collections.Generic

[<CLIMutable>]
type AlbumRepository = {
    [<Key>]
    AlbumId: int
    Title: string
    ArtistId: int
}