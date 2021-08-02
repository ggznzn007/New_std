using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;

/*C#을 이용해 Excel 파일에 데이타를 읽거나 쓸 때, Excel Automation을 이용하거나 OLEDB를 이용할 수 있다.

(1) C#에서 엑셀 오토메이션을 이용하기 위해서는 Excel Interop 을 참조한 후,
    Office Automation COM API들을 사용하게 된다.
(2) C#에서 엑셀을 OLEDB로 이용할 경우에는 ADO.NET의 OleDb 클래스들을 사용하여 엑셀 데이타를 핸들링하게 된다.*/

/*C#에서 Excel Automation 사용

엑셀 오토메이션은 COM API들이고, C# 에서 이를 엑세스하기 위해서는 먼저 Excel COM Interop DLL을 참조해야 한다. (주: Excel COM Dll 참조는 버젼에 따라 다른데, (1) Add Reference - COM -Type Libraries 에서 Microsoft Excecl Object Library 를 선택하여 참조하거나 (2) Add Reference - Assembly - Extensions에서 Microsoft.Office.Interop.Excel.dll 을 참조한다.)
DLL이 참조된 후에 using Microsoft.Office.Interop.Excel; 와 같이 Excel 네임스페이스를 참조하여 사용하게 된다.
프로그램 코드에서는 기본적으로 Excel의 구조대로 먼저 Excel Application 객체를 얻고,
Workbook 객체 그리고 Worksheet 객체를 차례로 얻은 후, 이 Worksheet의 Cell 혹은
Range를 지정하여 데이타를 핸들링한다.
마지막으로 사용 후에는 Excel 객체들을 Release해 주어야 백그라운드에 Excel.exe가 남지 않는다.*/

public class ExcelTest
{
    public static void RunTest()
    {
        List<string> testData = new List<string>()
            { "Excel", "Access", "Word", "OneNote" };

        Excel.Application excelApp = null;
        Excel.Workbook wb = null;
        Excel.Worksheet ws = null;

        try
        {
            // Excel 첫번째 워크시트 가져오기                
            excelApp = new Excel.Application();
            wb = excelApp.Workbooks.Add();
            ws = wb.Worksheets.get_Item(1) as Excel.Worksheet;

            // 데이타 넣기
            int r = 1;
            foreach (var d in testData)
            {
                ws.Cells[r, 1] = d;
                r++;
            }

            // 엑셀파일 저장
            wb.SaveAs(@"C:\temp\test.xls", Excel.XlFileFormat.xlWorkbookNormal);
            wb.Close(true);
            excelApp.Quit();
        }
        finally
        {
            // Clean up
            ReleaseExcelObject(ws);
            ReleaseExcelObject(wb);
            ReleaseExcelObject(excelApp);
        }
    }

    private static void ReleaseExcelObject(object obj)
    {
        try
        {
            if (obj != null)
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
        }
        catch (Exception ex)
        {
            obj = null;
            throw ex;
        }
        finally
        {
            GC.Collect();
        }
    }
}
