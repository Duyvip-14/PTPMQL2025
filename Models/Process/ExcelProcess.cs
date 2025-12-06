
namespace DemoMvc551.Models.Process;
using System;
using System.Data;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;

    public class ExcelProcess
    {
    [Obsolete]
    static ExcelProcess()
        {
            // Set license cho EPPlus (mục đích học tập/non-commercial)
           ExcelPackage.License.SetNonCommercialPersonal("N.Khanh Duy");
        }

        /// <summary>
        /// Đọc Excel trực tiếp từ IFormFile (file upload) -> trả về DataTable
        /// </summary>
        public static DataTable ExcelToDataTable(IFormFile file, bool hasHeader = true)
        {
            if (file == null) throw new ArgumentNullException(nameof(file));
            using var stream = file.OpenReadStream();
            return ExcelToDataTable(stream, hasHeader);
        }

       
        public static DataTable ExcelToDataTable(Stream stream, bool hasHeader = true)
        {
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            using var package = new ExcelPackage(stream);
            var ws = package.Workbook.Worksheets.FirstOrDefault();
            if (ws == null)
                throw new InvalidOperationException("Không tìm thấy worksheet trong file Excel.");

            var dt = new DataTable();

            var start = ws.Dimension.Start;
            var end = ws.Dimension.End;

            int startRow = start.Row;
            int endRow = end.Row;
            int startCol = start.Column;
            int endCol = end.Column;

            // Tạo cột (dựa vào header hoặc Column1..N)
            if (hasHeader)
            {
                for (int col = startCol; col <= endCol; col++)
                {
                    var header = ws.Cells[startRow, col].Text?.Trim();
                    if (string.IsNullOrEmpty(header))
                        header = $"Column{col}";

                    // đảm bảo tên cột không trùng
                    var unique = header;
                    int idx = 1;
                    while (dt.Columns.Contains(unique))
                    {
                        unique = $"{header}_{idx}";
                        idx++;
                    }
                    dt.Columns.Add(unique, typeof(string));
                }
                startRow++; // dữ liệu bắt đầu từ hàng kế tiếp
            }
            else
            {
                for (int col = startCol; col <= endCol; col++)
                    dt.Columns.Add($"Column{col}", typeof(string));
            }

            // Đọc dữ liệu từng hàng
            for (int row = startRow; row <= endRow; row++)
            {
                var dr = dt.NewRow();
                bool hasValue = false;
                for (int col = startCol; col <= endCol; col++)
                {
                    var cell = ws.Cells[row, col];
                    object? value = null;

                    if (cell?.Value == null)
                    {
                        value = DBNull.Value;
                    }
                    else
                    {
                        // Lấy dạng Text để dễ mapping (đã format như trong Excel)
                        if (cell.Value is DateTime dtVal)
                        {
                            value = dtVal.ToString("yyyy-MM-dd HH:mm:ss");
                        }
                        else if (cell.Value is double d)
                        {
                            // có thể là số hoặc ngày dưới dạng OADate
                            var fmt = (cell.Style.Numberformat?.Format ?? string.Empty).ToLower();
                            if (fmt.Contains("yy") || fmt.Contains("dd") || fmt.Contains("mm") || fmt.Contains("h") )
                            {
                                try
                                {
                                    value = DateTime.FromOADate(d).ToString("yyyy-MM-dd HH:mm:ss");
                                }
                                catch
                                {
                                    value = d.ToString();
                                }
                            }
                            else
                            {
                                value = cell.Text; // giữ text (có format như hiển thị)
                            }
                        }
                        else
                        {
                            value = cell.Text;
                        }
                    }

                    dr[col - startCol] = value ?? DBNull.Value;
                    if (value != null && value != DBNull.Value && !string.IsNullOrWhiteSpace(value.ToString()))
                        hasValue = true;
                }

                // bỏ qua row rỗng
                if (hasValue)
                    dt.Rows.Add(dr);
            }

            return dt;
        }
    }

