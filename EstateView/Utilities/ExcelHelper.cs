using EstateView.ViewModel.InstallmentSale;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;

namespace EstateView.Utilities
{
    public class ExcelHelper
    {
        private Microsoft.Office.Interop.Excel.Application xlApp;
        private Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        private Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
        private object misValue;
        private string filePath;

        public ExcelHelper()
        {
            this.filePath = "EstateView-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            this.xlApp = new Microsoft.Office.Interop.Excel.Application();
            this.misValue = System.Reflection.Missing.Value;
            this.xlWorkBook = xlApp.Workbooks.Add(misValue);
            this.xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)this.xlWorkBook.Worksheets.get_Item(1);
        }

        private void InitDocument(string docType)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Save " + docType + " Excel";
            saveFileDialog1.DefaultExt = "xlsx";
            saveFileDialog1.Filter = "Excel Files (*.xlsx) | *.xlsx";
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.OverwritePrompt = false;
            saveFileDialog1.FileName = docType + " - " + DateTime.Now.ToString("yyyyMMdd-HHmmss");
            if (saveFileDialog1.ShowDialog() == true)
            {
                this.filePath = saveFileDialog1.FileName;
            }
        }

        private void SaveAndCloseFile()
        {
            this.xlWorkBook.SaveAs(this.filePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault, this.misValue, this.misValue, this.misValue, this.misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges, this.misValue, this.misValue, this.misValue, this.misValue);
            this.xlWorkBook.Close(true, this.misValue, this.misValue);
            this.xlApp.Quit();

            Marshal.ReleaseComObject(this.xlWorkSheet);
            Marshal.ReleaseComObject(this.xlWorkBook);
            Marshal.ReleaseComObject(this.xlApp);
        }

        internal void CreateExcelFromInstallmentSaleProjections(List<InstallmentSaleProjectionViewModel> projections)
        {
            InitDocument("Installment Sale");
            SetHeaderRowForInstallmentSale();
            SetInstallmentSaleGridData(projections);
            FormatInstallmentSaleTable(projections);
            SaveAndCloseFile();
            OpenFile();
        }

        private void OpenFile()
        {
            try
            {
                var startInfo = new System.Diagnostics.ProcessStartInfo(this.filePath);
                startInfo.UseShellExecute = true;
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to Generate File.\nError: " + e.Message);
            }
        }

        private void FormatInstallmentSaleTable(List<InstallmentSaleProjectionViewModel> projections)
        {
            var projectionsEnd = (projections.Count + 2).ToString();

            // create and format grantor's estate table
            var grantorEstateTable = this.xlWorkSheet.get_Range("A2", "K" + projectionsEnd);
            this.xlWorkSheet.ListObjects.Add(Microsoft.Office.Interop.Excel.XlListObjectSourceType.xlSrcRange, grantorEstateTable,
                Type.Missing, Microsoft.Office.Interop.Excel.XlYesNoGuess.xlYes, Type.Missing).Name = "Grantor's Estate";
            this.xlWorkSheet.ListObjects["Grantor's Estate"].TableStyle = "TableStyleMedium16";

            // create and format grantor trust table
            var grantorTrustTable = this.xlWorkSheet.get_Range("L2", "T" + projectionsEnd);
            this.xlWorkSheet.ListObjects.Add(Microsoft.Office.Interop.Excel.XlListObjectSourceType.xlSrcRange, grantorTrustTable,
                Type.Missing, Microsoft.Office.Interop.Excel.XlYesNoGuess.xlYes, Type.Missing).Name = "Grantor Trust";
            this.xlWorkSheet.ListObjects["Grantor Trust"].TableStyle = "TableStyleMedium18";
        }

        private void SetInstallmentSaleGridData(List<InstallmentSaleProjectionViewModel> projections)
        {
            var row = 3;
            foreach (var projection in projections)
            {
                this.xlWorkSheet.Cells[row, 1] = projection.Projection.Year;
                this.xlWorkSheet.Cells[row, 2] = projection.Projection.EstateStartingAssets;
                this.xlWorkSheet.Cells[row, 3] = projection.Projection.EstateStartingAssetsWithoutNote;
                this.xlWorkSheet.Cells[row, 4] = projection.Projection.EstateAssetsGrowth;
                this.xlWorkSheet.Cells[row, 5] = projection.Projection.EstateSpendingSavingAmount;
                this.xlWorkSheet.Cells[row, 6] = projection.Projection.NotePayment;
                this.xlWorkSheet.Cells[row, 7] = 0 - projection.Projection.TrustIncomeTaxPaidByGrantor;
                this.xlWorkSheet.Cells[row, 8] = projection.Projection.TaxableEstate;
                this.xlWorkSheet.Cells[row, 9] = projection.Projection.LifetimeExclusionAvailable;
                this.xlWorkSheet.Cells[row, 10] = projection.Projection.EstateTaxLiability;
                this.xlWorkSheet.Cells[row, 11] = projection.Projection.EstateTaxSavingsOverNoPlanning;
                this.xlWorkSheet.Cells[row, 12] = projection.Projection.TrustStartingAssets;
                this.xlWorkSheet.Cells[row, 13] = projection.Projection.NoteBalance;
                this.xlWorkSheet.Cells[row, 14] = projection.Projection.TrustAssetsGrowth;
                this.xlWorkSheet.Cells[row, 15] = 0 - projection.Projection.TrustIncomeTaxPaidByTrust;
                this.xlWorkSheet.Cells[row, 16] = 0 - projection.Projection.NotePayment;
                this.xlWorkSheet.Cells[row, 17] = projection.Projection.TrustBalance;
                this.xlWorkSheet.Cells[row, 18] = projection.Projection.TrustBalanceUponDeath;
                this.xlWorkSheet.Cells[row, 19] = projection.Projection.Year;
                this.xlWorkSheet.Cells[row, 20] = projection.Projection.Notes;
                row++;
            }

            // format red text for payments and income tax
            this.xlWorkSheet.Range["G3:G" + row.ToString()].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Crimson);
            this.xlWorkSheet.Range["J3:J" + row.ToString()].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Crimson);
            this.xlWorkSheet.Range["O3:O" + row.ToString()].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Crimson);
            this.xlWorkSheet.Range["P3:P" + row.ToString()].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Crimson);

            // format green
            this.xlWorkSheet.Range["B3:B" + row.ToString()].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkGreen);
            this.xlWorkSheet.Range["C3:C" + row.ToString()].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkGreen);
            this.xlWorkSheet.Range["K3:K" + row.ToString()].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkGreen);

            // format as money
            this.xlWorkSheet.Range["B3:R" + row.ToString()].NumberFormat = "[$$-en-US] #,##0; ([$$-en-US] #,##0)";

            // databar
            Microsoft.Office.Interop.Excel.Databar databar1 = this.xlWorkSheet.Range["B3:B" + row.ToString()].FormatConditions.AddDatabar();
            Microsoft.Office.Interop.Excel.FormatColor formatColor1 = databar1.BarColor;
            databar1.BarFillType = Microsoft.Office.Interop.Excel.XlDataBarFillType.xlDataBarFillSolid;
            databar1.MinPoint.Modify(Microsoft.Office.Interop.Excel.XlConditionValueTypes.xlConditionValueNumber, 0);
            formatColor1.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);

            Microsoft.Office.Interop.Excel.Databar databar2 = this.xlWorkSheet.Range["C3:C" + row.ToString()].FormatConditions.AddDatabar();
            Microsoft.Office.Interop.Excel.FormatColor formatColor2 = databar2.BarColor;
            databar2.BarFillType = Microsoft.Office.Interop.Excel.XlDataBarFillType.xlDataBarFillSolid;
            databar2.MinPoint.Modify(Microsoft.Office.Interop.Excel.XlConditionValueTypes.xlConditionValueNumber, 0);
            formatColor2.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);

            Microsoft.Office.Interop.Excel.Databar databar3 = this.xlWorkSheet.Range["J3:J" + row.ToString()].FormatConditions.AddDatabar();
            Microsoft.Office.Interop.Excel.FormatColor formatColor3 = databar3.BarColor;
            databar3.BarFillType = Microsoft.Office.Interop.Excel.XlDataBarFillType.xlDataBarFillSolid;
            databar3.MinPoint.Modify(Microsoft.Office.Interop.Excel.XlConditionValueTypes.xlConditionValueNumber, 0);
            formatColor3.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Pink);

            Microsoft.Office.Interop.Excel.Databar databar4 = this.xlWorkSheet.Range["K3:K" + row.ToString()].FormatConditions.AddDatabar();
            Microsoft.Office.Interop.Excel.FormatColor formatColor4 = databar4.BarColor;
            databar4.BarFillType = Microsoft.Office.Interop.Excel.XlDataBarFillType.xlDataBarFillSolid;
            databar4.MinPoint.Modify(Microsoft.Office.Interop.Excel.XlConditionValueTypes.xlConditionValueNumber, 0);
            formatColor4.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.LightGreen);
        }

        private void SetHeaderRowForInstallmentSale()
        {
            // table titles
            this.xlWorkSheet.Range["A1:K1"].Merge();
            this.xlWorkSheet.Range["A1:K1"].BorderAround(
                Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous, 
                Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium, 
                Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic);
            this.xlWorkSheet.Range["A1:K1"].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            this.xlWorkSheet.Range["A1:K1"].Font.Bold = true;
            this.xlWorkSheet.Range["A1:K1"].Font.Size = 18;
            this.xlWorkSheet.Range["A1:K1"].Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            this.xlWorkSheet.Range["A1:K1"].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.DarkBlue);
            this.xlWorkSheet.Cells[1, 1] = "Grantor's Estate";

            this.xlWorkSheet.Range["L1:T1"].Merge();
            this.xlWorkSheet.Range["L1:T1"].BorderAround(
                Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
                Microsoft.Office.Interop.Excel.XlBorderWeight.xlMedium,
                Microsoft.Office.Interop.Excel.XlColorIndex.xlColorIndexAutomatic);
            this.xlWorkSheet.Range["L1:T1"].Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
            this.xlWorkSheet.Range["L1:T1"].Font.Bold = true;
            this.xlWorkSheet.Range["L1:T1"].Font.Size = 18;
            this.xlWorkSheet.Range["L1:T1"].Cells.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            this.xlWorkSheet.Range["L1:T1"].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.SlateGray);
            this.xlWorkSheet.Cells[1, 12] = "Grantor Trust";

            // table headers
            this.xlWorkSheet.Cells[2, 1] = "Year";
            this.xlWorkSheet.Cells[2, 2] = "Assets\n(w/ Note)";
            this.xlWorkSheet.Cells[2, 3] = "Assets\n(w/o Note)";
            this.xlWorkSheet.Cells[2, 4] = "Growth\n(after Taxes)";
            this.xlWorkSheet.Cells[2, 5] = "Annual Saving\n(or Spending)";
            this.xlWorkSheet.Cells[2, 6] = "Payment";
            this.xlWorkSheet.Cells[2, 7] = "Income Tax on\nTrust Income";
            this.xlWorkSheet.Cells[2, 8] = "Taxable Estate";
            this.xlWorkSheet.Cells[2, 9] = "Exclusion";
            this.xlWorkSheet.Cells[2, 10] = "Estate Tax\nLiability";
            this.xlWorkSheet.Cells[2, 11] = "Estate Tax\nSavings Over No Planning";
            this.xlWorkSheet.Cells[2, 12] = "Assets";
            this.xlWorkSheet.Cells[2, 13] = "Note Balance";
            this.xlWorkSheet.Cells[2, 14] = "Growth";
            this.xlWorkSheet.Cells[2, 15] = "Income Tax on\nTrust Income";
            this.xlWorkSheet.Cells[2, 16] = "Payment";
            this.xlWorkSheet.Cells[2, 17] = "Net Assets";
            this.xlWorkSheet.Cells[2, 18] = "Net Assets if\nGrantor Dies This Year";
            this.xlWorkSheet.Cells[2, 19] = "Year";
            this.xlWorkSheet.Cells[2, 20] = "Notes";
        }
    }
}

