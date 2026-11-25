# BẢN ĐẶC TẢ NÂNG CẤP & GAP ANALYSIS (UPGRADE SPECIFICATION)
## DỰ ÁN: SUPER MARIO 2D PLATFORMER (ĐỀ TÀI #36)
**Học phần:** Lập trình hướng đối tượng (MI4090) – K69 – Đại học Bách khoa Hà Nội  
**Giảng viên phụ trách:** TS. Trần Ngọc Thăng – Khoa Toán – Tin (FAMI / SAMI)  
**Tiêu chuẩn kiểm định:** Phiên bản 2026 – Khung Đánh giá Hai chiều A ↔ B & Hệ thống 5 Khóa Chặn  

---

## 1. TỔNG QUAN HIỆN TRẠNG VS. MỤC TIÊU ĐẠT CHUẨN A (GAP ANALYSIS)

### 1.1. Hiện trạng mã nguồn hiện tại (`Super_Mario_Bros_Clone-main`)
- **Gameplay cơ bản:** Di chuyển, nhảy, gia tốc, trọng lực, camera cuộn màn hình (`MarioMovementControl.cs`).
- **Nội dung màn chơi:** Duy nhất **01 Scene màn 1-1 hard-code** trong Unity Editor (`Super Mario Bros. (Main) Scene.unity`). Không có tệp dữ liệu ngoài (JSON/XML).
- **Lưu trữ dữ liệu:** Lưu điểm và mạng tạm thời trên RAM qua `ScoreData.cs` (ScriptableObject). **0% Save/Load bền vững** (tắt game bật lại mất toàn bộ tiến trình, không lưu tọa độ x/y, quái, thời gian).
- **Kiến trúc:** Mô hình nguyên khối (Monolith) trong Unity. **100% script phụ thuộc `UnityEngine` và `MonoBehaviour`**, không có phân tầng kiến trúc Core thuần C#.
- **Design Pattern:** 
  - `MarioStateMachine.cs` và `KoopaStateMachine.cs` đã manh nha State Pattern nhưng dính chặt vào `MonoBehaviour` và animation.
  - `EnemyFactory.cs` vi phạm nghiêm trọng nguyên lý OCP (sử dụng câu lệnh `switch(enemyType)` và ép kiểu `case GoombaBehavior:`).
- **Kiểm thử tự động:** **0 Unit Test, 0 Integration Test**.
- **Chất lượng mã nguồn:** Tồn tại dead code (thư mục `NOT RELEVANT, DELETE/`, tệp rỗng `PowerUps\.cs`, tệp crash `mono_crash.0.0.json`, tệp `.DS_Store`).

### 1.2. Mục tiêu Chuẩn Điểm A (Xuất sắc 9.5 – 10.0đ) & Tránh Cổng Chặn
- **Cổng G_Product:** Đạt **PASS (Trần 10.0đ)** – Chạy mượt Clean-room ≤ 3 phút, đủ ≥5 PCU (1 TV) hoặc ≥7–10 PCU (2 TV), Save/Load khôi phục 100% trạng thái.
- **Cổng G_Epistemic:** Đạt **PASS (Trục II ≥ 5.0/6.0đ, Trần 10.0đ)** – Kiến trúc 3 tầng tách rời `Game.Core` (0% Engine), bộ test tự động độc lập 100% PASS < 1s, vượt qua toàn bộ bài kiểm tra sống: Nhiễu loạn (Δ), Đánh đổi (W), Live OCP (2–3 phút tắt AI) và Live Fault-Injection.

### 1.3. Bảng Ma trận Đối chiếu Định lượng Định mức K69
| Chỉ số định lượng | Hiện trạng mã nguồn | Định mức Nhóm 01 người (1 TV) | Định mức Nhóm 02 người (2 TV) |
| :--- | :---: | :---: | :---: |
| **Nội dung chơi (PCU)** | 1 màn (Scene Unity) | **≥5 PCU** (4 màn JSON + 1 Boss) | **≥7–10 PCU** (7 màn JSON hoặc 5 màn + 2 chế độ) |
| **Use Cases cốt lõi** | 3 UC (Play, Pause, Lose) | **≥5 Use Cases** trọn vòng đời | **≥8 Use Cases** (chia đều TV1 & TV2) |
| **Thực thể Domain (Entity)** | 4 lớp MonoBehaviour | **≥8 lớp** Entity/VO/Enum thực chất | **≥13–15 lớp** Entity/VO/Enum thực chất |
| **Sơ đồ lớp UML 2.5** | 0 bản vẽ | **≥10–12 lớp** (đủ 6 quan hệ UML) | **≥16–20 lớp** (01 sơ đồ tổng duy nhất) |
| **Thuật toán & AI lõi** | 0 (Dùng Unity PhysX) | **≥2 thuật toán** có giả mã & $O(\cdot)$ | **≥4 thuật toán** (mỗi TV ≥2 thuật toán) |
| **GoF Design Patterns** | 1 mẫu lỗi (Factory switch) | **≥2 mẫu** thuộc **≥2 nhóm GoF** | **≥3 mẫu** (đủ nhóm hoặc có cầu nối TV1↔TV2) |
| **Kiểm thử tự động** | **0 test** | **≥10 Unit Tests** Pure Logic AAA | **≥18–20 Unit Tests** (≥9/TV, Mock chéo) |
| **Integration Test & Bug Log** | 0 | ≥4 ca test, Playtest ≥8, Bug Log ≥4 | ≥8 ca test, Playtest ≥15, Bug Log ≥6 |
| **Lịch sử Git & Đóng góp** | 1 commit unzipped | ≥12 commits rải ≥3 tuần | ≥24 commits rải ≥3 tuần, blame 40%–60% |

---

## 2. ĐẶC TẢ CHI TIẾT NÂNG CẤP THEO 20 TIÊU CHÍ (1.1 → 5.3)

---

### TRỤC I: HOÀN THIỆN SẢN PHẨM (4.00 ĐIỂM – 40%)

#### Tiêu chí 1.1: Quy mô nội dung chơi (PCU) & Use Case cốt lõi
- **Yêu cầu đạt A:**
  - *Nhóm 1 TV:* ≥5 PCU gồm ≥4 màn JSON tăng dần độ khó + 1 Boss Bowser Arena; ≥5 Use Cases trọn vòng đời.
  - *Nhóm 2 TV:* ≥7–10 PCU (≥7 màn JSON hoặc 5 màn chiến dịch + 2 chế độ: Endless Runner / Boss Rush); ≥8 Use Cases chia đều TV1 & TV2.
- **Danh mục 5 Màn chơi JSON (PCU):**
  1. `level_01.json` (Mushroom Plains - Đồng cỏ): Làm quen điều khiển cơ bản, quái Goomba tuần tra thẳng, khối gạch thường và khối ? chứa tiền.
  2. `level_02.json` (Underground Cavern - Hầm ngầm): Địa hình trần hẹp, hố sâu, Koopa mai rùa lăn phản xạ tường, khối gạch chứa Starman.
  3. `level_03.json` (Athletic Treetop - Bật nhảy trên mây): Nền tảng chuyển động (Moving Platforms), quái đuổi bắt khoảng cách (`AggressiveChaseStrategy`), vực thẳm sâu.
  4. `level_04.json` (Castle Maze - Mê cung Lâu đài): Chướng ngại vật lửa xoay (Firebars), dung nham, quái Piranha Plant phun cầu lửa.
  5. `level_05_boss.json` (Bowser's Lair - Đấu trường Boss): Sàn đấu có cầu dung nham, Boss Bowser đa pha (ném lửa, dậm đất), rìu công tắc (Axe Switch) để hạ cầu thả Boss.
- **Danh mục Use Cases:**
  - `UC01: StartGame_LevelSelect` (Chọn màn 1..5 từ danh mục nạp qua JSON).
  - `UC02: MoveAndJump` (Điều khiển Mario chạy, nhảy gia tốc, trọng lực, trượt đà).
  - `UC03: PowerUpEvolution` (Thu thập Nấm, Hoa lửa, Sao để biến hình và đổi kỹ năng).
  - `UC04: CombatAndStomp` (Nhảy giẫm quái, đá mai rùa, bắn cầu lửa tiêu diệt kẻ địch).
  - `UC05: PauseAndResume` (Tạm dừng, hiển thị menu cấu hình, đóng băng thời gian game).
  - `UC06: SaveProgress` (Lưu trọn vẹn 100% snapshot ván chơi ra slot JSON).
  - `UC07: LoadProgress` (Khôi phục toàn bộ trạng thái game từ tệp JSON).
  - `UC08: GameOverAndVictory` (Xử lý hết mạng/thời gian hoặc kéo cờ/hạ Bowser, cập nhật HighScore).

#### Tiêu chí 1.2: Save/Load đủ trạng thái, Dữ liệu hóa màn chơi & Bảng điểm
- **Yêu cầu đạt A:** Đọc dữ liệu từ file ngoài không cần biên dịch lại; Save/Load khôi phục bền vững 100% trạng thái (vị trí, máu, điểm, quái, block) sau khi tắt mở app.
- **Schema JSON Màn chơi (`Data/Levels/level_01.json`):**
```json
{
  "$schema": "http://json-schema.org/draft-07/schema#",
  "levelId": "LV01",
  "levelName": "Mushroom Plains",
  "timeLimit": 400,
  "theme": "Overworld",
  "gridSize": { "width": 120, "height": 15 },
  "playerSpawn": { "x": 3.0, "y": 2.0 },
  "goalFlag": { "x": 110.0, "y": 2.0 },
  "enemies": [
    { "type": "Goomba", "position": { "x": 12.0, "y": 2.0 }, "aiStrategy": "Patrol" },
    { "type": "Koopa", "position": { "x": 28.0, "y": 2.0 }, "aiStrategy": "Patrol" }
  ],
  "blocks": [
    { "id": "B01", "type": "QuestionBlock", "content": "SuperMushroom", "position": { "x": 10.0, "y": 5.0 } },
    { "id": "B02", "type": "BrickBlock", "content": "Coin", "position": { "x": 12.0, "y": 5.0 } }
  ]
}
```
- **Schema JSON Save Game (`Data/Saves/save_slot_1.json`):**
```json
{
  "slotId": 1,
  "savedTimestamp": "2026-11-19T20:30:00Z",
  "levelId": "LV01",
  "score": 2450,
  "coins": 38,
  "lives": 3,
  "remainingTime": 285.5,
  "player": {
    "form": "Super",
    "position": { "x": 34.5, "y": 2.0 },
    "velocity": { "x": 4.2, "y": 0.0 },
    "isInvincible": false,
    "invincibilityTime": 0.0,
    "facingRight": true
  },
  "activeEnemies": [
    { "id": "E02", "type": "Koopa", "position": { "x": 42.0, "y": 2.0 }, "state": "Walking", "isAlive": true }
  ],
  "triggeredBlockIds": ["B01"]
}
```
- **Bảng điểm cao (`Data/highscores.json`):** Mảng lưu Top 10 kỷ lục `{ "rank": 1, "playerName": "AAA", "score": 12500, "levelReached": "LV05", "date": "2026-11-19" }`.
- **Lớp kỹ thuật:** `ISaveStorage`, `JsonSaveStorage`, `SaveManager`, `LevelLoader`.

#### Tiêu chí 1.3: Chiều sâu nội dung chơi & Cân bằng độ khó (Balancing)
- **Yêu cầu đạt A:** Có ≥4 loại Enemy/Unit khác hành vi AI; ≥4 loại Item/Skill có tương tác combo cơ chế riêng; Bảng cân bằng thông số (Balancing Table) nạp từ file JSON ngoài.
- **Chi tiết 4 loại Enemy AI:**
  1. `Goomba`: Tốc độ 2.0 m/s, HP 1, tuần tra đảo chiều khi chạm tường (`PatrolAIStrategy`). Chết ngay khi bị giẫm từ trên xuống.
  2. `Koopa Troopa`: Tốc độ 2.5 m/s, HP 1. Khi bị giẫm thụt vào mai rùa (`ShellState`). Khi Mario đá mai, mai rùa lăn với tốc độ 12.0 m/s (`MovingShellStrategy`), dội tường và tiêu diệt mọi quái trên đường lăn.
  3. `Piranha Plant`: Quái hoa trong ống cống, chu kỳ trồi lên 3s / lặn 2s, phát hiện Mario trong bán kính 2.5m thì không trồi lên (`AmbushStrategy`). Miễn nhiễm nhảy giẫm, chỉ bị hạ bởi Fireball hoặc Mai rùa.
  4. `Bowser Boss`: Máu 5 HP, chia 3 pha AI (`BowserBossStrategy`):
     - *Pha 1 (HP 4-5):* Tuần tra khoảng hẹp, nhảy dậm đất chu kỳ 4s.
     - *Pha 2 (HP 2-3):* Khạc cầu lửa tầm xa (`FireballAttack`) về hướng Mario.
     - *Pha 3 (HP 1 - Enrage):* Tốc độ tăng x1.5, nhảy áp sát và phun lửa liên tục.
- **Chi tiết 4 loại Item/Skill & Combo:**
  1. `Super Mushroom`: Mario Nhỏ -> Mario Lớn (Tăng hitbox, đập vỡ được khối gạch Brick, chịu thêm 1 đòn sát thương).
  2. `Fire Flower`: Mario Lớn -> Fire Mario (Mở khóa skill nhấn phím `J` bắn cầu lửa `Fireball` nảy đất).
  3. `Starman`: Kích hoạt trạng thái bất tử trong 10 giây (Tốc độ tăng 20%, chạm mọi quái là quái bị tiêu diệt ngay lập tức).
  4. `Ice Flower / Mini Mushroom (Combo)`: Cầu băng đóng băng quái thành khối băng (`FrozenState`), Mario có thể đẩy khối băng trượt tiêu diệt Bowser hoặc nhảy lên khối băng để với tới vùng bí mật.
- **Bảng Cân bằng Thông số (Balancing Table - Nạp từ `Data/balance_config.json`):**
| Thực thể / Vật phẩm | HP | Vận tốc (m/s) | Sát thương gây ra | Điểm thưởng | Tương tác đặc biệt / Combo |
| :--- | :---: | :---: | :---: | :---: | :--- |
| **Mario (Small)** | 1 hit | Đi: 4.5, Chạy: 7.0 | 0 (Nhảy đè: 1) | — | Chạm quái mất mạng |
| **Mario (Super)** | 2 hits | Đi: 4.5, Chạy: 7.0 | 0 (Nhảy đè: 1) | — | Đập vỡ gạch BrickBlock |
| **Mario (Fire)** | 2 hits | Đi: 4.5, Chạy: 7.0 | Đạn lửa: 1 | — | Bắn tối đa 2 cầu lửa trên màn |
| **Mario (Star)** | Bất tử 10s | Đi: 5.5, Chạy: 8.5 | Chạm là diệt: $\infty$| 100/200/400/800.. | Tiêu diệt liên tiếp nhân đôi điểm |
| **Goomba** | 1 | 2.0 | 1 hit | 100 | Bị giẫm bẹp sau 0.5s |
| **Koopa Troopa** | 1 | Đi: 2.5, Mai: 12.0 | Chạm: 1 hit | 200 (Mai: 400) | Mai rùa dội tường diệt quái |
| **Piranha Plant** | 2 | 0 (Thò thụt) | 1 hit | 400 | Kháng nhảy giẫm |
| **Bowser Boss** | 5 | 1.8 (Enrage: 2.8) | Cầu lửa / Chạm: 1 | 5000 | Rút cầu dung nham hạ tức thì |

#### Tiêu chí 1.4: Đóng gói Clean-Room, Hồ sơ bàn giao & Git thực chất
- **Quy chuẩn Đóng gói Clean-Room:**
  - Build bản phát hành chạy độc lập `SuperMario2D_v2026.exe` (hoặc Unity WebGL build).
  - Chạy sạch ngay lập tức trên máy lạ (không cài Unity, chỉ cần .NET Runtime nếu là C# Core) trong vòng **≤ 3 phút**.
  - Toàn bộ đường dẫn asset, level, save là đường dẫn tương đối (Relative Path).
- **Hồ sơ Bàn giao đầy đủ:**
  - `README.md`: Hướng dẫn cài đặt, cấu hình phím điều khiển, lệnh chạy 100% Unit Test (`dotnet test`).
  - `Slide Báo cáo (.pptx)`: 12 slides chuẩn theo hướng dẫn Thầy Thăng.
  - `Video Demo (3–5 phút)`: Ghi hình chuỗi Title → Gameplay 5 màn → Boss Fight → Save/Load → Unit Test Runner.
- **Quy chuẩn Git:**
  - Nhóm 1 TV: ≥12 commits rải đều ≥3 tuần.
  - Nhóm 2 TV: ≥24 commits rải đều ≥3 tuần; tỷ lệ đóng góp mã logic qua `git blame` nằm trong khoảng **40% – 60%**.
  - `.gitignore` chuẩn Unity/Visual Studio: Cấm tuyệt đối commit `Library/`, `Temp/`, `obj/`, `bin/`, `Build/`, `*.userprefs`.

---

#### Tiêu chí 2.1: Vòng đời Gameplay trọn vẹn, UI/HUD & Game Feel
- **Luồng Màn hình (Scene Flow):**  
  `MainMenuScene` ──(Play)──> `LevelSelectScene` ──> `GameplayHUD (Active Arena)` ──(ESC/P)──> `PauseModal`  
  - Trong Pause Modal: Nút `Resume`, `Save Game`, `Load Game`, `Settings (Volume/SFX)`, `Quit to Menu`.
  - Kết thúc màn: `VictoryScene` (Kéo cờ / Hạ Boss) hoặc `GameOverScene` (Hết mạng / Hết giờ) ──> `HighScoreScene` ──> `Replay`.
- **Cơ chế Replay sạch:** Khi nhấn Replay, thu hồi toàn bộ đối tượng về Object Pool, reset sạch bộ đếm thời gian, điểm số và tải lại snapshot từ đầu màn mà không rò rỉ bộ nhớ.
- **Game Feel (Hình ảnh & Âm thanh):**
  - Hiệu ứng thị giác: Mario nhấp nháy trong suốt (Invincibility Frames) 2.0s khi mất máu; Screen Shake nhẹ khi đập đầu vào gạch hoặc Bowser nhảy dậm; hạt vỡ (Particle FX) khi phá gạch.
  - Âm thanh: Quản lý qua `SoundFXManager` có lưu cấu hình volume bền vững ra JSON, gồm BGM nền và ≥6 SFX (Jump, Stomp, PowerUp, PowerDown, Fireball, BowserRoar).

#### Tiêu chí 2.2: Độ ổn định End-to-End, Xử lý Ngoại lệ & Chống lỗi Gameplay
- **Cây Ngoại lệ CustomException thuần C#:**
```csharp
namespace SuperMario.Core.Exceptions
{
    public class MarioGameException : Exception
    {
        public MarioGameException(string message) : base(message) { }
    }
    public class SaveCorruptedException : MarioGameException
    {
        public string SlotPath { get; }
        public SaveCorruptedException(string path, string reason) 
            : base($"Tệp lưu tại {path} bị hỏng hoặc sai cấu trúc: {reason}") { SlotPath = path; }
    }
    public class InvalidLevelDataException : MarioGameException
    {
        public string LevelId { get; }
        public InvalidLevelDataException(string levelId, string details) 
            : base($"Dữ liệu màn chơi {levelId} không hợp lệ: {details}") { LevelId = levelId; }
    }
    public class EntityOutOfBoundsException : MarioGameException
    {
        public EntityOutOfBoundsException(string entityId) 
            : base($"Thực thể {entityId} rơi ra ngoài ranh giới bản đồ.") { }
    }
}
```
- **Cam kết 0 catch {} rỗng:** Mọi khối `try-catch` trong `SaveManager` và `LevelLoader` đều ghi log chi tiết hoặc hiển thị dialog thông báo lỗi thân thiện, tuyệt đối không nuốt lỗi.
- **Chống lỗi Gameplay (Anti-Tunneling & Softlock):** Dùng Raycast Continuous Collision Detection để ngăn đạn và Mario vận tốc cao xuyên qua gạch (Tunneling). Mario rơi xuống vực `Y < -5.0f` lập tức kích hoạt mất mạng, không rơi vô tận.

#### Tiêu chí 2.3: Hiệu năng thực chạy & Cấu hình ngoài
- **Độc lập FPS:** Toàn bộ tính toán vật lý, gia tốc, trọng lực nhân chuẩn xác với `deltaTime` (Fixed Timestep 60 FPS).
- **Object Pool tối ưu Zero GC Spike:** Cài đặt generic `ObjectPool<T>` tái sử dụng:
  - `FireballPool` (10 viên đạn lửa).
  - `ScorePopUpPool` (15 chữ số hiển thị điểm).
  - `GoombaPool` & `KoopaPool` (tái sinh quái khi vào tầm camera).
- **Đường dẫn tương đối:** Sử dụng `Path.Combine(Application.streamingAssetsPath, "Data/...")`, cấm tiệt đường dẫn tuyệt đối `C:\...`.

#### Tiêu chí 2.4: Chất lượng mã nguồn, Clean Code & Quy ước tổ chức
- **Chuẩn đặt tên Microsoft C#:** Lớp & Hàm: `PascalCase`, Thuộc tính: `PascalCase`, Biến private: `_camelCase`, Hằng số: `UPPER_SNAKE_CASE`, Interface: tiền tố `I`.
- **DRY (Don't Repeat Yourself):** Dùng chung lớp cơ sở `CharacterEntity` cho cả Mario và Enemy; dùng chung `GenericPool<T>` thay vì viết lại pool cho từng loại đạn.
- **Kiểm toán sạch:** Xóa sạch thư mục `NOT RELEVANT, DELETE`, tệp `PowerUps\.cs` rỗng, `.DS_Store`, không để sót mã thừa do AI sinh.

---

### TRỤC II: ĐỘ HIỂU SÂU PHÂN TÍCH – THIẾT KẾ – KIỂM THỬ ↔ CODE (6.00 ĐIỂM – 60%)

---

#### Tiêu chí 3.1: Phân rã Luật chơi, Use Case & Vòng lặp Game Loop ↔ Code
- **[A → B] Ánh xạ Game Loop:**
  - Vòng lặp 4 bước: `1. ProcessInput(IInputProvider)` → `2. FixedUpdate(dt)` (Vật lý & RuleEngine) → `3. CheckWinLoss & Events` → `4. RenderFrame & Audio`.
  - Bộ điều phối `GameSession` và `MarioRuleEngine` thuần C#:
```csharp
public class GameSession
{
    private readonly MarioRuleEngine _ruleEngine;
    private readonly MarioEntity _player;
    private readonly List<EnemyEntity> _enemies;
    public float RemainingTime { get; private set; }
    public GameState State { get; private set; }

    public void Tick(float dt)
    {
        if (State != GameState.Playing) return;
        RemainingTime -= dt;
        if (RemainingTime <= 0) { _ruleEngine.ResolveTimeUp(_player); State = GameState.GameOver; return; }
        _player.Update(dt);
        foreach (var enemy in _enemies) enemy.Update(dt);
        _ruleEngine.ProcessCollisions(_player, _enemies);
        if (_player.Lives <= 0) State = GameState.GameOver;
    }
}
```
- **[B → A] Truy vết ngược:** Mọi nhánh if trong `MarioRuleEngine.ProcessCollisions` đều tương ứng 1-1 với bảng luật chơi: giẫm từ trên xuống thì quái chết, chạm ngang thì Mario mất máu/thu nhỏ, Mario có Sao thì quái bay màu.

#### Tiêu chí 3.2: Mô hình Miền, Thực thể Game & Dữ liệu hóa ↔ Entity/VO/Enum (≥8–13 lớp)
- **Danh mục Thực thể miền cốt lõi thuần C#:**
  1. `Entity` (Abstract class): Vị trí `Vector2D`, vận tốc `Vector2D`, BoundingBox, cờ `IsActive`.
  2. `CharacterEntity` : `Entity`: Máu `CurrentHealth`, Tốc độ `MoveSpeed`, phương thức `TakeDamage(int)`.
  3. `MarioEntity` : `CharacterEntity`: Trạng thái `IMarioState`, số mạng `Lives`, số xu `Coins`, điểm `Score`.
  4. `EnemyEntity` : `CharacterEntity`: Chiến lược AI `IAIStrategy`, phương thức `OnStomped()`.
  5. `GoombaEntity` : `EnemyEntity`.
  6. `KoopaEntity` : `EnemyEntity`: Trạng thái mai rùa `IKoopaState`.
  7. `BowserBossEntity` : `EnemyEntity`: Máu tối đa 5, ném lửa, nhảy dậm.
  8. `BlockEntity` : `Entity`: Loại khối (`Question`, `Brick`), vật phẩm bên trong, trạng thái vỡ/nảy.
  9. `PowerUpEntity` : `Entity`: Loại vật phẩm (`Mushroom`, `Flower`, `Star`), vận tốc trượt.
  10. `Vector2D` (Value Object): Tọa độ 2D thuần C# bất biến (không dùng `UnityEngine.Vector2`).
  11. `BoundingBox` (Value Object): Tọa độ Min, Max phục vụ kiểm tra AABB.
  12. `GameSession` (Domain Controller): Điều phối toàn bộ phiên chơi.
  13. `MarioRuleEngine` (Domain Service): Chứa toàn bộ quy tắc tính điểm, va chạm, thắng thua.

#### Tiêu chí 3.3: Thuật toán Chơi & AI (≥2–4 thuật toán) + Phân tích $O(\cdot)$
- **Thuật toán 1: Kiểm tra Va chạm AABB & Tunneling Sweep Guard (`AABBCollisionSolver`):**
  - *Mô tả:* Kiểm tra giao nhau của hai trục độc lập X và Y: $\text{OverlapX} = (\min(A.x2, B.x2) - \max(A.x1, B.x1)) > 0$. Nếu vận tốc lớn, dùng thuật toán swept raycast để phát hiện thời điểm va chạm $t \in [0, 1]$.
  - *Độ phức tạp:* $O(1)$ cho từng cặp đối tượng; $O(N)$ khi kiểm tra Mario với danh sách $N$ quái lân cận (hoặc $O(\log N)$ nếu dùng Spatial Grid).
  - *Trường hợp biên:* Đạn bay vận tốc cực cao không bị lọt qua vách gạch hẹp (Tunneling).
- **Thuật toán 2: AI Tuần tra & Nhận diện Vực thẳm (`PatrolAIStrategy`):**
  - *Mô tả:* Quái di chuyển theo hướng `Direction` (-1 hoặc +1). Mỗi frame kiểm tra va chạm tường phía trước hoặc không có mặt đất dưới chân (Ledge detection) để tự động đảo chiều `Direction *= -1`.
  - *Độ phức tạp:* $O(1)$ thời gian, $O(1)$ bộ nhớ.
  - *Trường hợp biên:* Quái bị kẹt giữa 2 bức tường hẹp (đảo chiều liên tục nhưng không crash).
- **Thuật toán 3: Trí tuệ Boss Bowser đa pha (`BowserBossStrategy`):**
  - *Mô tả:* State Machine AI 3 pha kết hợp khoảng cách Euclidean $d = |P_{Mario} - P_{Bowser}|$. Nếu $d > 8.0m$ kích hoạt ném lửa tầm xa; nếu $d \le 4.0m$ kích hoạt nhảy dậm chấn động đất; khi HP < 30% chuyển sang trạng thái Enrage.
  - *Độ phức tạp:* $O(1)$ mỗi frame.
- **Thuật toán 4: Thuật toán Combo Điểm liên hoàn (`ComboScoringCalculator`):**
  - *Mô tả:* Khi Mario giẫm quái liên tiếp mà không chạm đất, điểm số được tính theo hàm mũ: $\text{Score} = 100 \times 2^{K-1}$ (100, 200, 400, 800, 1000, 2000, 4000, 8000, từ lần thứ 8 trở đi thưởng 1-Up).
  - *Độ phức tạp:* $O(1)$ tính toán.

---

#### Tiêu chí 4.1: Sơ đồ lớp UML (≥12–20 lớp) & Đủ 6 quan hệ chuẩn
- **Quy chuẩn bản vẽ:** Đúng chuẩn UML 2.5 với `<<interface>>`, `{abstract}`, visibility (+ public, - private, # protected), static gạch chân.
- **Đủ 6 quan hệ UML trong mã nguồn:**
  1. *Inheritance (<|—):* `CharacterEntity <|— MarioEntity`, `CharacterEntity <|— EnemyEntity`.
  2. *Realization (<|..):* `IMarioState <|.. SuperMarioState`, `ISaveStorage <|.. JsonSaveStorage`.
  3. *Composition (◆—):* `GameSession ◆— 1 MarioRuleEngine` (Khởi tạo `new` bên trong `GameSession`, hủy cùng nhau).
  4. *Aggregation (◇—):* `GameSession ◇— 0..* EnemyEntity` (Nhận danh sách quái nạp từ `LevelLoader` bên ngoài).
  5. *Association (——):* `MarioEntity —— 1 IMarioState` (Field tham chiếu `private IMarioState _state`).
  6. *Dependency (..>):* `MarioRuleEngine ..> CollisionResult` (Truyền dưới dạng tham số phương thức, không lưu thành field).

#### Tiêu chí 4.2: Sơ đồ hành vi động (≥2–4 sơ đồ) ↔ Kiểm chứng Debugger
- **Sơ đồ 1: State Machine Diagram – Tiến hóa Mario:**
  - Trạng thái: `SmallMario`, `SuperMario`, `FireMario`, `StarInvincible`, `Dead`.
  - Guard: `[Lives > 0]`, `[HasFireFlower]`, `[StarTimer <= 0]`.
- **Sơ đồ 2: Sequence Diagram – 1 Tick Cập nhật Game Loop:**
  - `GameSession.Tick(dt)` → `MarioEntity.Update(dt)` → `MarioRuleEngine.ProcessCollisions()` → `IHudObserver.OnScoreChanged()`.
- **Kiểm chứng Breakpoint:** Đặt Breakpoint tại `MarioEntity.ChangeState()`, cửa sổ Call Stack trên Visual Studio hiển thị 100% khớp chuỗi lifeline trong Sequence Diagram.

#### Tiêu chí 4.3: Kiến trúc phân tầng (≥3 tầng) & Kiểm toán using
- **Package Diagram 3 tầng:**
```
[ Presentation Layer: SuperMario.App (Unity Engine) ]
   ├── Views (MarioView, EnemyView, TilemapView)
   ├── InputAdapter (Unity New Input System -> IInputProvider)
   └── AudioAdapter (SoundFXManager -> IAudioService)
             │  (depends via Interfaces)
             ▼
[ Core Logic Layer: SuperMario.Core (Pure C# .NET Library - 0% UnityEngine) ]
   ├── Entities (MarioEntity, EnemyEntity, BlockEntity)
   ├── Rules (MarioRuleEngine, GameSession)
   ├── Patterns (State, Strategy, Factory, Observer)
   └── Ports (IInputProvider, ISaveStorage, IHudObserver)
             │  (depends via Interfaces)
             ▼
[ Infrastructure / Data Layer: SuperMario.Data ]
   ├── JsonLevelLoader (Đọc level_01.json)
   ├── JsonSaveStorage (Ghi/Đọc save_slot_1.json)
   └── HighScoreRepository (highscores.json)
```
- **Kiểm toán using sạch:** File trong `SuperMario.Core/` cam kết **KHÔNG CÓ DÒNG `using UnityEngine;`**. Biên dịch ra file `.dll` độc lập và kiểm thử tự động không cần mở Unity.

#### Tiêu chí 4.4: Đóng gói (Bất biến), Đa hình & LSP (Cross-Module LSP)
- **Bảo vệ Bất biến (Invariants):**
  - Máu và Mạng: `0 <= Lives <= 99`. Setter đóng gói private, chỉ thay đổi qua `LoseLife()` hoặc `AddLife()`.
  - Danh sách Quái: Trả về `IReadOnlyList<EnemyEntity>` để bên ngoài không thể `Clear()` hoặc thêm phần tử trái phép.
- **Đa hình & LSP:**
  - Gọi `enemy.TakeDamage(1)` hoặc `enemy.UpdateAI(dt)` thông qua Dynamic Binding.
  - **Cấm tiệt:** Ép kiểu `if (enemy is GoombaBehavior)` hay switch-case theo Type.

#### Tiêu chí 4.5: SOLID Cấu trúc (SRP, ISP, DIP/DI)
- **SRP (Single Responsibility):** Tách `GameManager` cồng kềnh thành:
  - `GameSession`: Quản lý vòng đời và thời gian.
  - `MarioRuleEngine`: Xử lý luật va chạm và tính điểm.
  - `SaveManager`: Phụ trách I/O tệp lưu.
- **ISP (Interface Segregation):** Tạo các interface nhỏ gọn: `IDamageable`, `IMovable`, `ICollectible`.
- **DIP (Dependency Inversion) & DI:** `GameSession` nhận `ISaveStorage` và `IInputProvider` qua Constructor Injection.

#### Tiêu chí 4.6: GoF Design Patterns (≥2–3 mẫu ≥2 nhóm) & Live OCP
- **Mẫu 1 (Behavioral) – State Pattern:** Quản lý trạng thái Mario (`IMarioState`: `SmallMarioState`, `SuperMarioState`, `FireMarioState`, `StarInvincibleState`).
- **Mẫu 2 (Behavioral) – Strategy Pattern:** Quản lý AI Quái & Boss (`IAIStrategy`: `PatrolAIStrategy`, `AggressiveChaseStrategy`, `BowserBossStrategy`).
- **Mẫu 3 (Creational) – Factory Method:** Khởi tạo thực thể từ JSON thông qua Creator đa hình hoặc Registry Pattern, **0% câu lệnh switch-case**.
- **Mẫu 4 (Behavioral) – Observer Pattern:** Cập nhật UI HUD và Âm thanh qua C# events / `IHudObserver`.
- **Sẵn sàng Live OCP (Thầy yêu cầu thêm quái mới trong 2-3 phút tắt AI):**
  - Thêm quái *Hammer Bro* ném búa: Tạo class `HammerBroStrategy : IAIStrategy`, đăng ký vào `EnemyFactory`. Không sửa 1 dòng code cũ nào!

---

#### Tiêu chí 5.1: Thiết kế Ca Test (EP/BVA) & Unit Test AAA (≥10–18 ca)
- **Ma trận Truy vết (Traceability Matrix):**
| Mã Test | Phương pháp thiết kế | Mục tiêu kiểm thử | Dòng mã nguồn Core kiểm chứng |
| :---: | :---: | :--- | :--- |
| `TC01` | Phân lớp tương đương (EP) | Mario Small nhận 1 damage -> Giảm 1 mạng, chết | `MarioEntity.TakeDamage()` |
| `TC02` | Phân lớp tương đương (EP) | Mario Super nhận 1 damage -> Thu nhỏ về Small, không mất mạng | `SuperMarioState.HandleDamage()` |
| `TC03` | Phân lớp tương đương (EP) | Mario Small ăn Super Mushroom -> Tiến hóa lên Super, +1000 điểm | `SmallMarioState.HandlePowerUp()` |
| `TC04` | Phân tích giá trị biên (BVA) | Mario tích lũy đủ 100 xu (từ 99 lên 100) -> Tăng 1 mạng, xu về 0 | `MarioEntity.CollectCoin()` |
| `TC05` | Phân tích giá trị biên (BVA) | Mario mạng chạm 0 (`Lives = 0`) -> Game chuyển sang trạng thái GameOver | `GameSession.Tick()` |
| `TC06` | Phân tích giá trị biên (BVA) | Hết thời gian (`RemainingTime <= 0`) -> Kích hoạt chết do timeout | `GameSession.Tick()` |
| `TC07` | Kiểm thử trạng thái (State) | Mario ăn Sao -> Kích hoạt StarInvincible, chạm quái là quái chết | `StarInvincibleState.HandleCollision()` |
| `TC08` | Kiểm thử logic va chạm | Mario nhảy giẫm lên đầu Goomba từ trên xuống -> Goomba chết, Mario nảy lên | `MarioRuleEngine.ResolveStomp()` |
| `TC09` | Kiểm thử ngoại lệ | Mario rơi xuống tọa độ $Y < -5.0$ -> Kích hoạt chết tức thì | `MarioRuleEngine.CheckPitFall()` |
| `TC10` | Kiểm thử logic mai rùa | Mario đá mai rùa Koopa nằm yên -> Mai rùa chuyển sang Moving, vận tốc tăng 12m/s | `KoopaEntity.KickShell()` |

- **Code mẫu Unit Test AAA thuần C# (xUnit/NUnit):**
```csharp
[Fact]
public void Mario_Collect100Coins_GivesExtraLife_AndResetsCoinsToZero()
{
    // Arrange
    var mario = new MarioEntity(initialLives: 3);
    for (int i = 0; i < 99; i++) mario.CollectCoin(1);

    // Act
    mario.CollectCoin(1);

    // Assert
    Assert.Equal(4, mario.Lives);
    Assert.Equal(0, mario.Coins);
}
```

#### Tiêu chí 5.2: Integration Test Save/Load, Playtest Matrix & Bug Log
- **Integration Tests:**
  - `SaveLoad_RoundTrip_PreservesAllState()`: Tạo phiên chơi, di chuyển Mario đến tọa độ (15.5, 3.0), ăn 35 xu, lưu vào `save_test.json`, nạp lại và kiểm tra khớp 100%.
  - `SaveLoad_CorruptedJson_ThrowsSaveCorruptedException()`: Cố ý làm hỏng cấu trúc tệp JSON, kiểm tra ném ngoại lệ `SaveCorruptedException`, không sập app.
- **Playtest Matrix (≥15 kịch bản):**
  - Chơi xuyên suốt 5 màn; Thử nghiệm bấm Pause liên tục; Thử nghiệm Save khi đang trên không; Thử nghiệm nhảy vào góc hẹp 2 khối gạch; v.v.
- **Bug Log chuẩn 6 cột (≥6 lỗi thật ghi nhận và sửa bằng Git):**
| Bug ID | Bước tái hiện | Root Cause | Commit sửa | Regression Test | Kết quả |
| :---: | :--- | :--- | :---: | :---: | :---: |
| `BUG-01` | Mario nhảy góc tường bị thụt vào trong gạch | Sai dấu tính toán bán kính BoundingBox | `git commit 4a12b3` | `TC_Collision_Corner_NoSnag` | PASS |
| `BUG-02` | Mai rùa Koopa đá dội tường văng ngược trúng Mario ngay frame 1 | Không có khoảng đệm delay kích hoạt hitbox | `git commit 8f92c1` | `TC_Koopa_KickShell_InvulnerableTime` | PASS |
| `BUG-03` | Ăn 100 xu mạng tăng nhưng số xu bị âm | Trừ xu trước khi cộng mạng sai thứ tự | `git commit c334d5` | `TC04_Mario_Collect100Coins` | PASS |
| `BUG-04` | Tải lại tệp save quái bị nhân đôi số lượng | Chưa dọn sạch danh sách quái cũ trước khi nạp | `git commit 1b77e2` | `TC_Integration_LoadClearsOldEnemies` | PASS |
| `BUG-05` | Mario bất tử ăn nấm bị mất hiệu ứng sao | Trạng thái State ghi đè không kiểm tra StarTimer | `git commit 9e44a1` | `TC_StarState_PowerUpPreservesStar` | PASS |
| `BUG-06` | Pause game làm âm thanh BGM chạy lặp vô tận | AudioManager không đồng bộ `AudioListener.pause` | `git commit 7d11f0` | `TC_Audio_PauseMutesAll` | PASS |

#### Tiêu chí 5.3: Live Fault-Injection & Kịch bản Debugger
- **Kịch bản Tiêm lỗi Sống (Fault-Injection):**
  - Tiêm lỗi 1: Đổi `mario.Lives <= 0` thành `mario.Lives < 0` trong `GameSession.cs`. Chạy `dotnet test` → Ca test `TC05` chuyển ĐỎ ngay lập tức.
  - Tiêm lỗi 2: Bỏ lưu trường `coins` trong `JsonSaveStorage.cs` → Ca test `SaveLoad_RoundTrip` chuyển ĐỎ ngay lập tức.
- **Kỹ năng Debugger:** Đặt Breakpoint tại hàm xử lý va chạm, mở Call Stack, Watch biến `_currentHealth` và sửa lỗi trực tiếp trước mặt giảng viên trong 2 phút.
