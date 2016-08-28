using System;
using System.Collections.Generic;
using System.Linq;
using Haven.Resources.Formats.Binary.Layers;
using NUnit.Framework;

namespace Haven.Resources.Formats.Binary
{
	[TestFixture]
	public class BinaryLayerHandlerTests
	{
		[Test]
		public void AudioDataTest()
		{
			var input = new AudioLayer {
				Data = new byte[] { 42, 10, 11, 12 }
			};

			var serializer = new AudioLayerHandler();
			var output = (AudioLayer)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.BaseVolume, Is.EqualTo(input.BaseVolume));
			Assert.That(output.Data, Is.EquivalentTo(input.Data));
		}

		[Test]
		public void Audio2DataTest()
		{
			var input = new AudioLayer
			{
				Id = "id",
				Data = new byte[] { 42, 10, 11, 12 },
				BaseVolume = 2.2
			};

			var serializer = new Audio2LayerHandler();
			var output = (AudioLayer)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.BaseVolume, Is.EqualTo(input.BaseVolume));
			Assert.That(output.Data, Is.EquivalentTo(input.Data));
		}

		[Test]
		public void ActionDataTest()
		{
			var input = new ActionLayer
			{
				Hotkey = 'h',
				Name = "Name",
				Parent = new ResourceRef("Parent", 42),
				Prerequisite = "Prerequisite",
				Verbs = new[] { "verb1", "verb2" }
			};

			var serializer = new ActionLayerHandler();
			var output = (ActionLayer)serializer.Reserialize(input);

			Assert.That(output.Hotkey, Is.EqualTo(input.Hotkey));
			Assert.That(output.Name, Is.EqualTo(input.Name));
			Assert.That(output.Parent.Name, Is.EqualTo(input.Parent.Name));
			Assert.That(output.Parent.Version, Is.EqualTo(input.Parent.Version));
			Assert.That(output.Prerequisite, Is.EqualTo(input.Prerequisite));
			Assert.That(output.Verbs, Is.EquivalentTo(input.Verbs));
		}

		[Test]
		public void AnimDataTest()
		{
			var input = new AnimLayer
			{
				Id = 42,
				Duration = 900,
				Frames = new short[] { 1, 2, 3, 4, 5 }
			};

			var serializer = new AnimLayerHandler();
			var output = (AnimLayer)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Duration, Is.EqualTo(input.Duration));
			Assert.That(output.Frames, Is.EquivalentTo(input.Frames));
		}

		[Test]
		public void CodeDataTest()
		{
			var input = new CodeLayer
			{
				Name = "java.package.class",
				ByteCode = new byte[] { 1, 2, 3, 4, 5 }
			};

			var serializer = new CodeLayerHandler();
			var output = (CodeLayer)serializer.Reserialize(input);

			Assert.That(output.Name, Is.EqualTo(input.Name));
			Assert.That(output.ByteCode, Is.EqualTo(input.ByteCode));
		}

		[Test]
		public void CodeEntryDataTest()
		{
			var input = new CodeEntryLayer
			{
				Entries = new[] {
					new CodeEntry("name1", "class.name1"),
					new CodeEntry("name2", "class.name2"),
					new CodeEntry("name3", "classname3")
				},
				Classpath = new [] {
					new ResourceRef("1", 2),
					new ResourceRef("2", 12),
				}
			};

			var serializer = new CodeEntryLayerHandler();
			var output = (CodeEntryLayer)serializer.Reserialize(input);

			Assert.That(output.Entries, Is.EqualTo(input.Entries));
			Assert.That(output.Classpath, Is.EqualTo(input.Classpath));
		}

		[Test]
		public void FoodEventTest()
		{
			var input = new FoodEventLayer {
				Color = Color.FromArgb(1, 2, 3, 4),
				Sort =  123,
				Name = "Namee"
			};
			var serializer = new FoodEventLayerHandler();
			var output = (FoodEventLayer)serializer.Reserialize(input);
			Assert.That(output.Color, Is.EqualTo(input.Color));
			Assert.That(output.Name, Is.EqualTo(input.Name));
			Assert.That(output.Sort, Is.EqualTo(input.Sort));
		}

		[Test]
		public void FontDataTest()
		{
			var input = new FontLayer
			{
				Type = 0,
				Data = new byte[] { 1, 2, 3, 4, 5 }
			};

			var serializer = new FontLayerHandler();
			var output = (FontLayer)serializer.Reserialize(input);

			Assert.That(output.Type, Is.EqualTo(input.Type));
			Assert.That(output.Data, Is.EquivalentTo(input.Data));
		}

		[Test]
		public void ImageDataTest()
		{
			var input = new ImageLayer {
				Id = 42,
				Offset = new Point2D(1, 2),
				Z = -10,
				SubZ = -20,
				Data = new byte[] { 1, 2, 3, 4, 5 },
				IsLayered = true
			};

			var serializer = new ImageLayerHandler();
			var output = (ImageLayer)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Z, Is.EqualTo(input.Z));
			Assert.That(output.SubZ, Is.EqualTo(input.SubZ));
			Assert.That(output.Offset, Is.EqualTo(input.Offset));
			Assert.That(output.IsLayered, Is.EqualTo(input.IsLayered));
			Assert.That(output.Data, Is.EquivalentTo(input.Data));
		}

		[Test]
		public void Material2LayerTest()
		{
			var input = new MaterialLayer {
				Id = 1234,
				IsLinear = false,
				IsMipmap = true,
				Parts = new [] {
					new MaterialLayer.Part { Name = "mlink", Properties = new object[] { "gfx/someres", 1, 2, 3 } },
					new MaterialLayer.Part { Name = "col", Properties = new object[] { 1, 2.0f, "dsd", new Point2D(1, 2)  } },
					new MaterialLayer.Part { Name = "mipmap", Properties = new object[0] },
				}
			};

			var serializer = new Material2LayerHandler();
			var output = (MaterialLayer)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.IsLinear, Is.EqualTo(input.IsLinear));
			Assert.That(output.IsMipmap, Is.EqualTo(input.IsMipmap));
			Assert.That(output.Parts, Is.Not.Null);
			Assert.That(output.Parts.Length, Is.EqualTo(input.Parts.Length));

			foreach (var inputMaterial in input.Parts)
			{
				var outputMaterial = output.Parts.FirstOrDefault(x => x.Name == inputMaterial.Name);
				Assert.That(outputMaterial, Is.Not.Null);
				Assert.That(outputMaterial.Properties, Is.EquivalentTo(inputMaterial.Properties));
			}
		}

		private static readonly MeshLayer[] MeshTestInputs = {
			new MeshLayer {
				Id = 42,
				Ref = 12,
				MaterialId = 13,
				Indexes = new short[] { 1, 6, 66 }
			},
			new MeshLayer {
				Id = 43,
				Ref = -1,
				RData = new Dictionary<string, string> {
					{ "key1", "value1" },
					{ "key2", "value2" }
				},
				MaterialId = 13,
				Indexes = new short[] { 1, 6, 66 }
			}
		};

		[Test]
		[TestCaseSource(nameof(MeshTestInputs))]
		public void MeshDataTest(MeshLayer input)
		{
			var serializer = new MeshLayerHandler();
			var output = (MeshLayer)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Ref, Is.EqualTo(input.Ref));
			Assert.That(output.MaterialId, Is.EqualTo(input.MaterialId));
			Assert.That(output.Indexes, Is.EquivalentTo(input.Indexes));
			Assert.That(output.RData, Is.EquivalentTo(input.RData));
		}

		[Test]
		public void MidiDataTest()
		{
			var input = new MidiLayer {
				Bytes = new byte[] { 1, 2, 3, 4, 5 }
			};

			var serializer = new MidiLayerHandler();
			var output = (MidiLayer)serializer.Reserialize(input);

			Assert.That(output.Bytes, Is.EquivalentTo(input.Bytes));
		}

		[Test]
		public void NegDataTest()
		{
			var input = new NegLayer
			{
				Center = new Point2D(12, 21),
				Hitbox = new Rect(1, 2, 3, 4),
				Sz = new Point2D(5, 6),
				Ep = new Point2D[8][]
			};
			input.Ep[2] = new[] { new Point2D(45, 54), new Point2D(46, 64) };
			input.Ep[6] = new[] { new Point2D(47, 74), new Point2D(48, 84) };

			var serializer = new NegLayerHandler();
			var output = (NegLayer)serializer.Reserialize(input);

			Assert.That(output.Center, Is.EqualTo(input.Center));
			Assert.That(output.Hitbox, Is.EqualTo(input.Hitbox));
			Assert.That(output.Sz, Is.EqualTo(input.Sz));
			for (int i = 0; i < 8; i++)
				Assert.That(output.Ep[i], Is.EqualTo(input.Ep[i]));
		}

		[Test]
		public void NinepatchDataTest()
		{
			var input = new NinepatchLayer
			{
				Top = 1,
				Bottom = 2,
				Left = 3,
				Right = 4
			};

			var serializer = new NinepatchLayerHandler();
			var output = (NinepatchLayer)serializer.Reserialize(input);

			Assert.That(output.Top, Is.EqualTo(input.Top));
			Assert.That(output.Bottom, Is.EqualTo(input.Bottom));
			Assert.That(output.Left, Is.EqualTo(input.Left));
			Assert.That(output.Right, Is.EqualTo(input.Right));
		}

		[Test]
		public void PoseLayerTest()
		{
			var input = new PoseLayer {
				Length = 1.2,
				Speed = 3.4,
				Id = 345,
				Flags = 255,
				Mode = 1,
				Effects = new [] {
					new PoseEffect { Events = new [] {
						new PoseEvent {
							Type = 1,
							Id = "id",
							Time = 5.6,
						},
						new PoseEvent {
							Type = 0,
							Time = -3.24,
							ResRef = new ResourceRef("test", 42),
							Data = new byte[] { 2, 3, 4, 5, 6 }
						}
					}}
				},
				Tracks = new [] {
					new PoseTrack() {
						BoneName = "bone",
						Frames = new [] {
							new PoseFrame {
								Time = 3.4,
								RotationAngle = 32.14,
								RotationAxis = new [] { -128.0, 22.1, 33.2 },
								Translation = new [] { -128.0, -3.2534, -22.12345 },
							}
						}
					}
				}
			};

			var serializer = new PoseLayerHandler();
			var output = (PoseLayer)serializer.Reserialize(input);

			var tolerance = 1.0 / Math.Pow(10, 6);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Length, Is.EqualTo(input.Length).Within(tolerance));
			Assert.That(output.Speed, Is.EqualTo(input.Speed).Within(tolerance));
			Assert.That(output.Flags, Is.EqualTo(input.Flags));
			Assert.That(output.Mode, Is.EqualTo(input.Mode));
			Assert.That(output.Effects.Length, Is.EqualTo(output.Effects.Length));
			Assert.That(output.Tracks.Length, Is.EqualTo(output.Tracks.Length));
		}

		[Test]
		public void SkeletonLayerTest()
		{
			var input = new SkeletonLayer {
				Bones = new [] {
					new SkeletonBone {
						Name = "test",
						Parent = "",
						Position = new float[] { 1, 2, 3 },
						RotationAngle = 42,
						RotationAxis = new float[] { 3, 4, 5 }
					},
					new SkeletonBone {
						Name = "test2",
						Parent = "test",
						Position = new float[] { 12, 12, 33 },
						RotationAngle = 42,
						RotationAxis = new float[] { 31, 24, 1.4f }
					},
				},
			};

			var serializer = new SkeletonLayerHandler();
			var output = (SkeletonLayer)serializer.Reserialize(input);

			Assert.That(output.Bones.Length, Is.EqualTo(input.Bones.Length));

			foreach (var inputBone in input.Bones)
			{
				var outputBone = output.Bones.FirstOrDefault(x => x.Name == inputBone.Name);
				Assert.That(outputBone, Is.Not.Null);
				Assert.That(outputBone.Parent, Is.EqualTo(inputBone.Parent));
				Assert.That(outputBone.RotationAngle, Is.EqualTo(inputBone.RotationAngle));
				Assert.That(outputBone.RotationAxis, Is.EquivalentTo(inputBone.RotationAxis));
				Assert.That(outputBone.Position, Is.EquivalentTo(inputBone.Position));
			}
		}

		[Test]
		public void TexDataTest()
		{
			var input = new TexLayer {
				Id = 42,
				ImageData = new byte[] { 23, 212, 21, 45 },
				MaskImageData = new byte[] { 223, 1, 34, 5 },
				Mipmap = TexMipmap.Dav,
				MagFilter = TexMagFilter.Nearest,
				MinFilter = TexMinFilter.NearestMipmapLinear,
				Offset = new Point2D(42, 24),
				Size = new Point2D(12, 21),
			};

			var serializer = new TexLayerHandler();
			var output = (TexLayer)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Mipmap, Is.EqualTo(input.Mipmap));
			Assert.That(output.MagFilter, Is.EqualTo(input.MagFilter));
			Assert.That(output.MinFilter, Is.EqualTo(input.MinFilter));
			Assert.That(output.Size, Is.EqualTo(input.Size));
			Assert.That(output.Offset, Is.EqualTo(input.Offset));
			Assert.That(output.ImageData, Is.EquivalentTo(input.ImageData));
			Assert.That(output.MaskImageData, Is.EquivalentTo(input.MaskImageData));
		}

		[Test]
		public void TextDataTest()
		{
			var input = new TextLayer
			{
				Text = "Text"
			};

			var serializer = new TextLayerHandler();
			var output = (TextLayer)serializer.Reserialize(input);

			Assert.That(output.Text, Is.EqualTo(input.Text));
		}

		[Test]
		public void TileDataTest()
		{
			var input = new TileLayer
			{
				Id = 42,
				Type = 'g',
				Weight = 123,
				ImageData = new byte[] { 1, 2, 3, 4, 6 }
			};

			var serializer = new TileLayerHandler();
			var output = (TileLayer)serializer.Reserialize(input);

			Assert.That(output.Id, Is.EqualTo(input.Id));
			Assert.That(output.Type, Is.EqualTo(input.Type));
			Assert.That(output.Weight, Is.EqualTo(input.Weight));
			Assert.That(output.ImageData, Is.EquivalentTo(input.ImageData));
		}

		[Test]
		public void TilesetDataTest()
		{
			var input = new TilesetLayer
			{
				FlavorDensity = 128,
				HasTransitions = true,
				FlavorObjects = new[] {
					new FlavorObjectData { Resource = new ResourceRef("res1", 1), Weight  = 1 },
					new FlavorObjectData { Resource = new ResourceRef("res2", 2), Weight  = 2 },
					new FlavorObjectData { Resource = new ResourceRef("res3", 3), Weight  = 3 },
				}
			};

			var serializer = new TilesetLayerHandler();
			var output = (TilesetLayer)serializer.Reserialize(input);

			Assert.That(output.FlavorDensity, Is.EqualTo(input.FlavorDensity));
			Assert.That(output.HasTransitions, Is.EqualTo(input.HasTransitions));
			Assert.That(output.FlavorObjects, Is.EquivalentTo(input.FlavorObjects));
		}

		[Test]
		public void Tileset2DataTest()
		{
			var input = new Tileset2Layer
			{
				TilerName = "name",
				FlavorDensity = 128,
				FlavorObjects = new[] {
					new FlavorObjectData { Resource = new ResourceRef("res1", 1), Weight  = 1 },
					new FlavorObjectData { Resource = new ResourceRef("res2", 2), Weight  = 2 },
					new FlavorObjectData { Resource = new ResourceRef("res3", 3), Weight  = 3 },
				},
				TilerAttributes = new object[] {
					new object[] { "attr1", 1, 1.0 },
					new object[] { "attr2", "attr2value"},
					new object[] { "attr3", 1, 2, 3, 4, 5}
				},
				Tags = new [] { "tag1", "tag2" }
			};

			var serializer = new Tileset2LayerHandler();
			var output = (Tileset2Layer)serializer.Reserialize(input);

			Assert.That(output.TilerName, Is.EqualTo(input.TilerName));
			Assert.That(output.FlavorDensity, Is.EqualTo(input.FlavorDensity));
			Assert.That(output.FlavorObjects, Is.EquivalentTo(input.FlavorObjects));
			Assert.That(output.TilerAttributes, Is.EquivalentTo(input.TilerAttributes));
			Assert.That(output.Tags, Is.EquivalentTo(input.Tags));
		}

		[Test]
		public void TooltipDataTest()
		{
			var input = new TooltipLayer
			{
				Text = "Text"
			};

			var serializer = new TooltipLayerHandler();
			var output = (TooltipLayer)serializer.Reserialize(input);

			Assert.That(output.Text, Is.EqualTo(input.Text));
		}

		[Test]
		public void Vertex2LayerTest()
		{
			var input = new VertexLayer {
				Flags = 8,
				VertexCount = 2,
				Positions = new float[2 * 3],
				Colors = new float[2 * 4],
				TexCoords = new float[2 * 2],
				Normals = new float[2 * 3],
				Bones = new VertexLayer.BoneArray {
					Mba = 5,
					Bones = new [] {
						new VertexLayer.Bone { Name = "123", Vertices = new [] {
							new VertexLayer.BoneVertex { Vn =  2, Weights = new float[2] }
						}}
					}
				}
			};

			var serializer = new Vertex2LayerHandler();
			var output = (VertexLayer)serializer.Reserialize(input);

			Assert.That(output.Flags, Is.EqualTo(input.Flags));
			Assert.That(output.VertexCount, Is.EqualTo(input.VertexCount));

			Assert.That(output.Positions, Is.EquivalentTo(input.Positions));
			Assert.That(output.Colors, Is.EquivalentTo(input.Colors));
			Assert.That(output.TexCoords, Is.EquivalentTo(input.TexCoords));
			Assert.That(output.Normals, Is.EquivalentTo(input.Normals));
			Assert.That(output.Tangents, Is.Null);
			Assert.That(output.Bitangents, Is.Null);

			Assert.That(output.Bones, Is.Not.Null);
			Assert.That(output.Bones.Mba, Is.EqualTo(input.Bones.Mba));
			Assert.That(output.Bones.Bones, Is.Not.Null);
			Assert.That(output.Bones.Bones.Length, Is.EqualTo(input.Bones.Bones.Length));
		}

		#region SpriteLink

		private static readonly SpriteLinkLayer[] SpriteLinkTestInput = {
			new SpriteLinkLayer {
				Version = 1,
				DefaultLinkId = 3,
				Links = new SortedDictionary<short, SpriteLink> {
					{1, new SpriteLink.Resource { Version = 2, Name = "test1"} },
					{2, new SpriteLink.Tile {
							DefaultLinkId = 2,
							SubLinks = new [] {
								new TileSubLink { Tag = "tag1", SubLinkId = 1 },
								new TileSubLink { Tag = "tag2", SubLinkId = 2 }
							}
						}
					},
					{3, new SpriteLink.Resource { Version = 2, Name = "test2"} }
				}
			}
		};

		[Test]
		[TestCaseSource(nameof(SpriteLinkTestInput))]
		public void SpriteLinkTest(SpriteLinkLayer input)
		{
			var serializer = new SpriteLinkLayerHandler();
			var output = (SpriteLinkLayer)serializer.Reserialize(input);

			Assert.That(output.Version, Is.EqualTo(input.Version));
			Assert.That(output.DefaultLinkId, Is.EqualTo(input.DefaultLinkId));
			Assert.That(output.Links.Count, Is.EqualTo(input.Links.Count));
		}

		#endregion
	}
}
