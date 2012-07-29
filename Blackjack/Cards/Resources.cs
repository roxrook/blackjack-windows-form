using System;
using System.Collections.Generic;
using System.Text;
using System.Resources;
using System.Reflection;
using System.Globalization;
using System.Drawing;

namespace Cards {
	static public class Resources {
		private static ResourceManager images;

		static Resources() {
			images = new ResourceManager("Cards.Images", Assembly.GetExecutingAssembly());
		}

		public static ResourceManager Images {
			get { return images; }
		}

		public static Image GetImage(int imageId) {
			return (Image)images.GetObject(imageId.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture);
		}

		public static Image GetImage(string imageId) {
			return (Image)images.GetObject(imageId, CultureInfo.InvariantCulture);
		}
	}
}
