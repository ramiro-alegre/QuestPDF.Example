using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

QuestPDF.Settings.License = LicenseType.Community;

var result = Document.Create(container =>
{
    container.Page(page =>
    {
        page.Size(PageSizes.A4);
        page.Margin(2, Unit.Centimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(20));
        
        page.Header()
            .Column(column => {
                column.Item().ShowOnce().Text("This text will be present on the first page").BackgroundColor(Colors.Blue.Lighten1);

                column.Item().AlignLeft().Text("AlignLeft");
                column.Item().AlignLeft().Text(text => {
                    text.Span("Text ").FontColor(Colors.Red.Accent1);
                    text.Span("With ").FontColor(Colors.Blue.Accent1);
                    text.Span("different ").Bold();
                    text.Span("characteristics").Underline();
                });
            });
        
        page.Content()
            .Column(x =>
            {
                x.Item().Table(table => {

                    table.ColumnsDefinition(headerTable => {
                        headerTable.ConstantColumn(width: 100); // <-- i know this column takes 100, because the value how i insert isn't large
                        headerTable.ConstantColumn(width: 100);
                        headerTable.RelativeColumn(); // <-- it takes all remaining space
                    });

                    table.Header(headerColumns => {
                        headerColumns.Cell().Column(1).Element(CellTable).Text("First Header").FontSize(10);
                        headerColumns.Cell().Column(2).Element(CellTable).Text("Second header").FontSize(10);
                        headerColumns.Cell().Column(3).Element(CellTable).Text("Last header").FontSize(10);
                    });

                    table.ExtendLastCellsToTableBottom();

                    for(uint CurrentRow = 1; CurrentRow <=50; CurrentRow++)
                    {
                        table.Cell().Row(CurrentRow).Column(1).Element(CellTable).Text("First value").FontSize(10);
                        table.Cell().Row(CurrentRow).Column(2).Element(CellTable).Text("Second value").FontSize(10);
                        table.Cell().Row(CurrentRow).Column(3).Element(CellTable).Text("Last value").FontSize(10);
                    }
                    
                    static IContainer CellTable(IContainer container)
                    {
                        return container
                            .Border(0.5f)
                            .AlignCenter()
                            .ShowEntire(); // <-- This helps me to show the entire row, and not allow it to appear cut off.
                    }
                });
            });
        
        page.Footer()
            .AlignRight()
            .Text(x =>
            {
                x.CurrentPageNumber();
                x.Span("/");
                x.TotalPages();
            });
    });
});

result.GeneratePdf("Example_1.pdf");

var MergedDocument = Document.Merge(
    documents: new List<Document>(){result, result, result}
)
.UseContinuousPageNumbers();

MergedDocument.GeneratePdf("Example_Merge_Document_2.pdf");


