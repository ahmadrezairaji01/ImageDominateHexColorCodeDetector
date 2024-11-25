// Using requirements.
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

//  Summary:
//      The main process of application.
const string imageName = "test.jpg";

// Find out root of project and handle error if it does not exist.
var executablePath = AppDomain.CurrentDomain.BaseDirectory;
var projectRoot = Directory.GetParent(executablePath)?.Parent?.Parent?.Parent?.FullName;
if (string.IsNullOrWhiteSpace(projectRoot))
    throw new Exception("Project root was not found!");

// Combine path of root and image name to find image path.
var imagePath = Path.Combine(projectRoot, imageName);

// Call detector method to get hex code of the dominate color.
var dominantColor = GetDominantColor(imagePath);

// Print result and return.
Console.WriteLine($"Dominant Color (Hex): #{dominantColor.R:X2}{dominantColor.G:X2}{dominantColor.B:X2}");
return 0;

//  Summary:
//      Find the dominate hex color code of image which exist in the path of input parameter.
static Rgb24 GetDominantColor(string imagePath)
{
    // Inject ImageSharp dependencies.
    using var image = Image.Load<Rgb24>(imagePath);

    // Resize the image to reduce computation.
    image.Mutate(x => x.Resize(50, 50));

    // Dictionary to hold color frequencies.
    var colorCount = new Dictionary<Rgb24, int>();

    // Iterate over each pixel.
    for (var y = 0; y < image.Height; y++)
    {
        for (var x = 0; x < image.Width; x++)
        {
            var color = image[x, y];
            if (!colorCount.TryAdd(color, 1))
                colorCount[color]++;
        }
    }

    // Find the most frequent color.
    var dominantColor = colorCount.MaxBy(c => c.Value).Key;

    // Return result.
    return dominantColor;
}


