namespace GiraffeChinook.Data

open Microsoft.EntityFrameworkCore

type ChinookContext(options: DbContextOptions<ChinookContext>) =
    inherit DbContext(options)
    
    [<DefaultValue>]
    val mutable artist : DbSet<ArtistRepository>

    member this.Artist
        with get() = this.artist
        and set (v: DbSet<ArtistRepository>) = this.artist <- v