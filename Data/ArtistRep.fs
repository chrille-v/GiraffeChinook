namespace GiraffeChinook.Data

open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type ArtistRepository = {
    [<Key>]
    ArtistId: int
    Name: string
}