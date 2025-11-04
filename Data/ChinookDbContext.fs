namespace GiraffeChinook.Data

open Microsoft.EntityFrameworkCore

type ChinookContext(options: DbContextOptions<ChinookContext>) =
    inherit DbContext(options)
    
    [<DefaultValue>]
    val mutable artist : DbSet<ArtistRepository>

    member this.Artist
        with get() = this.artist
        and set (v: DbSet<ArtistRepository>) = this.artist <- v

    [<DefaultValue>]
    val mutable album : DbSet<AlbumRepository>

    member this.Album
        with get() = this.album
        and set (v: DbSet<AlbumRepository>) = this.album <- v