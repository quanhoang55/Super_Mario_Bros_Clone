#!/usr/bin/env bash
# Script biên dịch LaTeX Beamer Slide - Super Mario Bros Clone 2D
set -e

DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
cd "$DIR"

echo "=== Đang biên dịch Slide LaTeX: main.tex ==="

# Ưu tiên XeLaTeX để hiển thị font chữ tiếng Việt sắc nét chuẩn OpenType/TrueType
if command -v /Library/TeX/texbin/xelatex &> /dev/null; then
    COMPILER="/Library/TeX/texbin/xelatex"
elif command -v xelatex &> /dev/null; then
    COMPILER="xelatex"
elif command -v /Library/TeX/texbin/pdflatex &> /dev/null; then
    COMPILER="/Library/TeX/texbin/pdflatex"
elif command -v pdflatex &> /dev/null; then
    COMPILER="pdflatex"
else
    echo "Lỗi: Không tìm thấy trình biên dịch xelatex hoặc pdflatex!"
    exit 1
fi

echo "Sử dụng trình biên dịch: $COMPILER"
"$COMPILER" -interaction=nonstopmode -halt-on-error main.tex
"$COMPILER" -interaction=nonstopmode -halt-on-error main.tex

echo "=== Biên dịch thành công! Tệp đầu ra: $DIR/main.pdf ==="
