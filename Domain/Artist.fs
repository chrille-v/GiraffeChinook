namespace GiraffeChinook.Domain

open GiraffeChinook.Data

type Artist = {
    ArtistId: int
    Name: string
    Album: AlbumRepository list
}