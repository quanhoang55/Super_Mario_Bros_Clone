# HƯỚNG DẪN BIÊN DỊCH VÀ CẤU TRÚC SLIDE LATEX BEAMER

## 1. Cấu trúc thư mục
- `main.tex`: Mã nguồn LaTeX Beamer chính (tỉ lệ 16:9, tiếng Việt chuẩn `babel`/`T5`, bộ màu ĐHBK Hà Nội & Super Mario).
- `main.pdf`: Bản PDF hoàn chỉnh đã biên dịch sẵn (10 slides chuẩn theo `content.md`).
- `compile.sh`: Script tự động biên dịch bằng `pdflatex`.
- `content.md`: Dàn ý gốc của bài thuyết trình.
- `assets/`: Thư mục chứa toàn bộ ảnh chụp minh họa thực tế của dự án.

## 2. Cách biên dịch lại
Chạy lệnh sau trong terminal:
```bash
cd /Users/hoanganhquan/Downloads/Super-Mario-Bros./docs/slide
./compile.sh
```
Hoặc dùng trực tiếp trình biên dịch LaTeX:
```bash
pdflatex -interaction=nonstopmode main.tex
```

## 3. Nội dung 10 Slides chuẩn hóa
1. **Slide 1:** Trang tiêu đề, thông tin học phần MI4090, phân công TV1 (Core) và TV2 (Infrastructure & Presentation) 50/50 git blame (`assets/start_game.png`).
2. **Slide 2:** Đặt vấn đề & Mục tiêu Đồ án (Khung kiểm định A $\leftrightarrow$ B, Rubric A+++) (`assets/life_scene.png`).
3. **Slide 3:** Phân rã Quy mô Màn chơi ($\ge 7$ PCUs) và Ma trận 8 Use Cases (`assets/in_game_1.png`).
4. **Slide 4:** Kiến trúc Phần mềm 3 Tầng tách rời (Clean Architecture, 0% UnityEngine ở Core) (`assets/edit_1.png`).
5. **Slide 5:** Mô hình Miền (Domain Model) & UML Class Diagram ($\ge 20$ lớp, đủ 6 quan hệ) (`assets/in_game_2.png`).
6. **Slide 6:** 5 Mẫu Thiết kế GoF (State, Factory Method, Observer, Command \& DIP, Object Pool) (`assets/in_game_3.png`).
7. **Slide 7:** Thuật toán Cốt lõi (Physics AABB Penetration Solver) & Trí tuệ Nhân tạo Enemy AI (`assets/in_game_1.png`).
8. **Slide 8:** Kiểm thử Phần mềm ($\ge 18$ Unit Tests xUnit AAA chạy độc lập < 1s) & Bug Log 6 lỗi (`assets/game_over.png`).
9. **Slide 9:** Quy chuẩn Mã nguồn (Naming Conventions C\#, Clean Code, SRP) (`assets/edit_1.png`).
10. **Slide 10:** Tổng kết, cam kết điểm A, diễn tập Vấn đáp Live Audit (Live OCP \& Live Fault-Injection) (`assets/win_game.png`).
