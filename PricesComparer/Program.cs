using PricesComparer;

var items = XmlGetter.GetInfo(args[0]);

if (items.Count == 0)
{
    File.WriteAllText("/home/oboi/Program/gkyudin/exchangeDirectory/botLog_" + Guid.NewGuid(), "нет позиций из xml c сайта для сравнения цен");
    return;
}
//JsonSerializerOptions options = new()
//{
//    WriteIndented = true,
//    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
//};

//Console.WriteLine(JsonSerializer.Serialize(items, options));

var csvDirPath = Path.GetFullPath("*");

var mask = args[1] switch
{
    "wp" => "*forSite_wallpapers_priceComparer*",

    "fw" => "*forSite_fretworks_priceComparer*",

    "fw3d" => "*forSite_fretworks_priceComparer*",

    _ => throw new NotSupportedException()
};

var filesPaths = Directory.GetFiles(csvDirPath, mask);

if (filesPaths.Length == 0)
{
    File.WriteAllText("*/botLog_" + Guid.NewGuid(), "нет выгрузки из бд для сравнения цен");
    return;
}

var csv = File.ReadAllLines(filesPaths[0]);

if (csv.Length <= 1)
{
    File.WriteAllText("*/botLog_" + Guid.NewGuid(), "в csv только 1 строка - заголовок");
    return;
}

List<string> badItemsResult = ["Название позиции;Цена БД;Цена Сайт;URL;ID"];

int badItemsCount = 0;

for (int i = 1; i < csv.Length; i++)
{
    var splitted = csv[i].Split(';');

    var id = splitted[0];

    var price = splitted[1];

    var discountPrice = splitted[2];

    if (items.TryGetValue(id, out var itemFromSite))
    {
        if ((discountPrice != "0" && discountPrice != itemFromSite.Price) ||
            (discountPrice == "0" && price != itemFromSite.Price))
        {
            badItemsCount++;
            badItemsResult.Add($"{itemFromSite.Name};{(discountPrice == "0" ? price : discountPrice)};{itemFromSite.Price};{itemFromSite.Url};{itemFromSite.Id}");
        }
    }
}

if (badItemsCount > 0)
{
    File.WriteAllLines("*/botFile_" + args[1] + "_сравнение_цен.csv", badItemsResult);
}