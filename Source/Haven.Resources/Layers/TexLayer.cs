namespace Haven.Resources
{
	/// <summary>
	/// Texture data.
	/// </summary>
	public class TexLayer
	{
		/// <summary>
		/// Texture ID.
		/// </summary>
		public short Id { get; set; }

		public Point2D Offset { get; set; }

		/// <summary>
		/// Texture size.
		/// </summary>
		public Point2D Size { get; set; }

		/// <summary>
		/// Texture image data.
		/// </summary>
		public byte[] ImageData { get; set; }

		/// <summary>
		/// Texture mask image data.
		/// </summary>
		public byte[] MaskImageData { get; set; }

		/// <summary>
		/// Mipmap generation algorithm to use for the texture.
		/// </summary>
		public TexMipmap Mipmap { get; set; }

		/// <summary>
		/// Magnification filter type used by the texture.
		/// </summary>
		public TexMagFilter MagFilter { get; set; }

		/// <summary>
		/// Minification filter type used by the texture.
		/// </summary>
		public TexMinFilter MinFilter { get; set; }
	}

	/// <summary>
	/// Mipmap generation algorithms.
	/// </summary>
	public enum TexMipmap
	{
		None,
		Average,
		Random,
		Cnt, // ??
		Dav, // ??
	}

	/// <summary>
	/// Texture magnification filter types.
	/// </summary>
	public enum TexMagFilter
	{
		Nearest,
		Linear,
	}

	/// <summary>
	/// Texture minification filter types.
	/// </summary>
	public enum TexMinFilter
	{
		Nearest,
		Linear,
		NearestMipmapNearest,
		NearestMipmapLinear,
		LinearMipmapNearest,
		LinearMipmapLinear,
	}
}
