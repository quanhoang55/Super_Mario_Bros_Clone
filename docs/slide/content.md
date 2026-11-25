# DÀN Ý SLIDE THUYẾT TRÌNH BÁO CÁO BÀI TẬP LỚN MI4090
## Đề tài: Super Mario Bros Clone 2D (Clean Architecture Edition)

---

### Slide 1: Trang tiêu đề & Thông tin Nhóm
* **Tiêu đề dự án:** Super Mario Bros Clone 2D – Clean Architecture & Design Patterns Edition.
* **Học phần:** Lập trình Hướng đối tượng (MI4090 – Viện Toán ứng dụng và Tin học – HUST).
* **Thành viên nhóm (Nhóm 02 người chia đều khối lượng):**
  * **TV1 (Trần Văn A – Leader):** Đóng góp 50% git blame; phụ trách `Game.Core` (State Machine Mario, Physics AABB Collision, Save/Load Schema JSON & Unit Tests Core).
  * **TV2 (Nguyễn Văn B – Member):** Đóng góp 50% git blame; phụ trách `Game.Infrastructure` & `Presentation` (Enemy AI System, Level TileMap Loader, UI Observer, AudioManager & Integration Tests).
* **Hồ sơ Git:** Repo nhóm rải đều $\ge$ 3 tuần với $\ge$ 24 commits chuẩn `.gitignore` Unity/VS.

---

### Slide 2: Đặt vấn đề & Mục tiêu Đồ án
* **Bối cảnh đề tài:** Tái hiện tựa game classic *Super Mario Bros* 2D bằng Unity 6 & C# bám sát nguyên tắc Lập trình Sạch (Clean Code).
* **Mục tiêu đáp ứng Rubric A+++:**
  * Đảm bảo tính nhất quán đối chiếu 2 chiều **A ↔ B** giữa Mô hình/Đặc tả (**A**) và Mã nguồn thực thi (**B**).
  * Tách bạch 3 tầng kiến trúc (`Game.Core` thuần C# không rò rỉ `UnityEngine.dll`), đảo ngược phụ thuộc Input qua Interface `IInputProvider`.
  * Đáp ứng đầy đủ chỉ tiêu khối lượng cho nhóm 2 người: $\ge$ 7–10 PCUs (5 màn + 2 chế độ chơi) và $\ge$ 8 Use Cases liên thông.

---

### Slide 3: Phân rã Quy mô Màn chơi (PCUs) & Danh mục 8 Use Cases
* **Danh mục 7 PCUs (5 Màn + 2 Chế độ chơi):**
  * World 1-1 (Grassland), World 1-2 (Underground), World 1-3 (Athletic), World 1-4 (Castle) và Level Select Map.
  * 2 Chế độ: Classic Mode (3 Lives) & Speedrun Boss Challenge.
* **Ma trận 8 Use Cases liên phân hệ (Chia đều TV1–TV2):**
  * **TV1:** UC01 Khởi tạo Session, UC03 Di chuyển & Biến hình Mario, UC04 Va chạm Physics AABB, UC07 Save/Load JSON.
  * **TV2:** UC02 Chọn màn chơi (Level Select), UC05 Combo Scoring, UC06 Pause/Cheat Codes (`Ctrl+R`, `P`), UC08 HighScore Board.

---

### Slide 4: Kiến trúc Phần mềm 3 Tầng (Package Diagram)
* **Cấu trúc Giải pháp 3 Tầng Tách biệt (Package Diagram):**
  * `Game.Core` (Class Library thuần C#): Chứa Entities (`Player`, `Enemy`, `Item`), `RuleEngine`, `MarioFSM`, `PhysicsCollisionSystem` và các `Interfaces` hợp đồng TV1–TV2.
  * `Game.Infrastructure`: Cài đặt `JsonLevelLoader`, `JsonSaveRepository` và `ObjectPoolManager`.
  * `Game.Presentation`: Adapter mỏng gồm `MonoBehaviours`, `SpriteRenderers`, `Tilemaps`, UI HUD Observer và `InputAdapter`.
* **Đối chiếu A ↔ B:** 0 rò rỉ framework UI trong Core, 0 phụ thuộc vòng (Circular Dependency), hỗ trợ chạy Headless Unit Tests độc lập.

---

### Slide 5: Mô hình Miền (Domain Model) & UML Class Diagram
* **UML Class Diagram Tổng thể:** 01 sơ đồ duy nhất chứa $\ge$ 20 lớp thực chất thể hiện đủ 6 quan hệ UML (*Association, Aggregation, Composition, Generalization, Realization, Dependency*).
* **Phân rã Thực thể Core ($\ge$ 13–16 lớp):**
  * Thực thể Nhân vật & Kẻ địch: `Player`, `Goomba`, `Koopa`, `PiranhaPlant`, `Lakitu`.
  * Thực thể Vật phẩm & Bản đồ: `Fireball`, `Coin`, `SuperMushroom`, `FireFlower`, `TileMap`.
  * Bộ quản lý trạng thái: `GameSession`, `RuleEngine`, `SaveData`.
* **Bảo vệ Bất biến Miền:** Khóa setter private (`private set`), sử dụng `IReadOnlyList` bảo vệ danh sách đối tượng.

---

### Slide 6: Các Mẫu Thiết kế GoF (Design Patterns)
* **State Pattern (Hành vi):** Quản lý biến hình Mario (`SmallMarioState`, `SuperMarioState`, `FireMarioState`, `DeadMarioState`) loại bỏ `if-else` lồng nhau.
* **Factory Method Pattern (Khởi tạo):** Khởi tạo động biến thể Kẻ địch (`EnemyFactory`) và Vật phẩm (`ItemFactory`) từ schema JSON cấu hình.
* **Observer Pattern (Hành vi):** Đăng ký sự kiện `OnScoreChanged`, `OnHealthChanged` giúp ngắt kết nối trực tiếp giữa Core với UI HUD và `AudioManager`.
* **Command Pattern & DIP (Hành vi/Cấu trúc):** Đóng gói thao tác phím từ Unity New Input System thông qua `IInputProvider` thành `JumpCommand`, `MoveCommand`, `FireCommand`.
* **Object Pool Pattern (Khởi tạo):** Tái sử dụng đạn lửa `Fireball` và hiệu ứng va chạm để tối ưu bộ nhớ, giữ FPS ổn định 60 FPS.

---

### Slide 7: Thuật toán Cốt lõi & Trí tuệ Nhân tạo (AI)
* **Thuật toán Va chạm Physics AABB (Axis-Aligned Bounding Box):** Tính toán va chạm hộp và giải quyết chồng lấp (Penetration Resolution), chống giật xuyên tường (tunneling).
* **Thuật toán AI Kẻ địch (FSM Patrol & Chasing AI):** Goomba di chuyển tuần tra đổi hướng khi gặp vật cản; Koopa nấp mai rùa & trượt mai; Lakitu thả đạn theo khoảng cách Player.
* **Dynamic JSON Level Loader:** Giải mã ma trận dữ liệu từ tệp `level_1_1.json` ngoài mà không cần biên dịch lại code.
* **Save Game Checksum & CustomException:** Mã hóa kiểm tra tính toàn vẹn tệp save, tự động kích hoạt `SaveCorruptedException` khi tệp bị hỏng.

---

### Slide 8: Kiểm thử Phần mềm & Đảm bảo Chất lượng (QA)
* **Ma trận Unit Tests Pure Logic ($\ge$ 18 Tests AAA):**
  * 9 Unit Tests của TV1: Kiểm thử FSM Mario, va chạm AABB và Save/Load schema.
  * 9 Unit Tests của TV2: Kiểm thử Enemy AI, Factory Method và Object Pool.
  * 100% Unit Tests xanh độc lập không mở Scene Unity.
* **Integration Tests & Bug Log ($\ge$ 8 Tests, $\ge$ 6 Bugs):**
  * Test tích hợp Save/Load khôi phục 100% trạng thái trận đấu sau khi ngắt app.
  * Trừ khử 6 lỗi thực tế ghi vết trong Bug Log: AABB softlock, Object Pool despawn null reference, Save file corruption, Animation state sync.

---

### Slide 9: Quy chuẩn Mã nguồn & Naming Convention
* **Quy chuẩn Đặt tên C# / .NET / Unity:**
  * Class, Struct, Enum, Method, Property: Sử dụng **PascalCase** (VD: `PlayerController`, `GameSession`).
  * Private fields: Sử dụng **_camelCase** (VD: `_moveSpeed`, `_currentHp`).
  * Interface: Tiền tố **I** + PascalCase (VD: `IInputProvider`, `ISaveRepository`).
* **Sạch mã nguồn (Clean Code):** 0 dead code, 0 hard-code đường dẫn tuyệt đối, tuân thủ nguyên tắc Single Responsibility Principle (SRP).

---

### Slide 10: Tổng kết & Kiểm định Hai chiều A ↔ B
* **Đánh giá tổng thể:** Đạt trần điểm Trục I (Hoàn thiện Sản phẩm 4.00đ / 40%) và Trục II (Độ hiểu sâu Phân tích – Thiết kế – Kiểm thử 6.00đ / 60%).
* **Sẵn sàng Vấn đáp & Live Audit tại chỗ:**
  * Trình diễn Live OCP: Thêm 01 loại Enemy/Item mới trực tiếp tại buổi bảo vệ mà không sửa code cũ.
  * Thao tác Debugger Call Stack & Live Fault-Injection chứng minh độ khớp 100% với sơ đồ Sequence/State Diagram.