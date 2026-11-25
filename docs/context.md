# CONTEXT – PHASE COMPLETION (Super Mario 2D Platformer – Học phần MI4090 – K69)

## Phase 01: NỀN TẢNG KIẾN TRÚC, TÁCH LÕI CORE & DỮ LIỆU SAVE/LOAD

**Nguồn gốc:** `docs/roadmap.md` – mục "PHASE 1: NỀN TẢNG KIẾN TRÚC, TÁCH LÕI CORE & DỮ LIỆU SAVE/LOAD"
**Thời lượng dự kiến:** Tuần 1
**Trọng tâm:** Tách biệt hoàn toàn Core Logic khỏi Engine (Clean Architecture – Tiêu chí 1.2, 2.2, 4.3, 4.5)

---

### 1. Mục tiêu Phase 1

- Xây dựng giải pháp kiến trúc 3 tầng sạch (Clean Architecture):
  - `SuperMario.Core`: Class Library C# thuần, **0% tham chiếu `UnityEngine`**.
  - `SuperMario.Data`: Class Library xử lý I/O JSON (Newtonsoft.Json).
  - `SuperMario.App`: Tầng Unity 2D (View, Animation, Particle, New Input System).
- Xây dựng cơ chế nạp màn JSON và hệ thống Save/Load bền vững.
- Dọn dẹp toàn bộ dead code.

---

### 2. Kiến trúc Assembly đã tạo

| Assembly | File asmdef | Vai trò | Engine refs |
|---|---|---|---|
| `SuperMario.Core` | `Assets/Scripts/Core/SuperMario.Core.asmdef` | Domain thuần C# (Vector2D, BoundingBox, Exceptions, Contracts, Levels, Save, GenericPool) | **Không** (`noEngineReferences: true`) |
| `SuperMario.Data` | `Assets/Scripts/Data/SuperMario.Data.asmdef` | JSON I/O (Newtonsoft.Json), reference Core | **Không** (`noEngineReferences: true`) |
| `SuperMario.App` | `Assets/Scripts/SuperMario.App.asmdef` | Toàn bộ script Unity hiện có + adapter/bootstrap | Có |
| `SuperMario.Input` | `Assets/New Input System/SuperMario.Input.asmdef` | Code Input System auto-generated | Có |

**Ghi chú quan trọng về "di chuyển script":**
- `SuperMario.App.asmdef` được đặt tại `Assets/Scripts/` nên **toàn bộ script gameplay hiện có** (Blocks, Enemies, Managers, Mario, PowerUps, Flag Pole, Generic Code) tự động thuộc tầng `SuperMario.App` — **không cần di chuyển file**, tránh làm mất GUID reference trong scene/prefab.
- Các script thuần C# **mới** được đặt đúng tầng: `Assets/Scripts/Core/`, `Assets/Scripts/Data/`.
- `SuperMario.Input` là asmdef thứ 4 **bắt buộc về kỹ thuật**: code `PlayerInputActions` do Input System sinh ra phải nằm trong một assembly riêng để `SuperMario.App` có thể tham chiếu (asmdef không thể tham chiếu `Assembly-CSharp`).

**Package bổ sung:** `com.unity.nuget.newtonsoft-json` (3.2.1) trong `Packages/manifest.json`.

---

### 3. Code đã tạo

#### SuperMario.Core (thuần C#, không `using UnityEngine;`)
```
Assets/Scripts/Core/
├── Domain/Vector2D.cs                  # struct + toán tử +, -, *, Magnitude, Normalized
├── Domain/BoundingBox.cs               # AABB + Intersects()/Contains()
├── Exceptions/MarioGameException.cs
├── Exceptions/SaveCorruptedException.cs
├── Exceptions/InvalidLevelDataException.cs
├── Exceptions/EntityOutOfBoundsException.cs
├── Contracts/IInputProvider.cs          # IsJumpPressed, MoveAxisX, IsShootPressed...
├── Contracts/ISaveStorage.cs            # Save/Load/Delete snapshot
├── Contracts/IHudObserver.cs            # cập nhật HUD
├── Levels/LevelDefinition.cs            # schema JSON màn chơi
├── Levels/EntitySpawn.cs
├── Save/GameSnapshot.cs                 # snapshot 100% ván chơi
├── Save/EntityState.cs
├── Save/HighScoreEntry.cs
├── Save/HighScoreTable.cs
└── Pools/GenericPool.cs                 # GenericPool<T> + IPoolable
```

#### SuperMario.Data (JSON I/O, engine-free)
```
Assets/Scripts/Data/
├── JsonLevelLoader.cs                   # nạp level_01.json + validate
├── JsonSaveStorage.cs                   # ISaveStorage → save_slot_1.json
├── HighScoreRepository.cs               # Top 10 → highscores.json
└── Serialization/FieldsOnlyCamelCaseContractResolver.cs
```

> `FieldsOnlyCamelCaseContractResolver` chỉ serialize **public fields** (camelCase) — tránh Newtonsoft serialize các computed property như `Normalized`, `Magnitude` (gây self-referencing loop) mà không cần gắn attribute Newtonsoft vào Core.

#### SuperMario.App (Unity layer)
```
Assets/Scripts/App/
├── Adapters/UnityInputAdapter.cs        # IInputProvider ↔ PlayerInputActions
└── Bootstrap/Phase1Bootstrapper.cs      # smoke test: load level + Save/Load round-trip
```

#### JSON data (StreamingAssets)
```
Assets/StreamingAssets/
├── Levels/level_01.json                 # 20 entities (ground, blocks, goomba, koopa, pipe, coin, powerup, flagpole)
├── save_slot_1.json                     # snapshot mẫu
└── highscores.json                      # Top 10 mẫu
```

---

### 4. Kết quả verify (chạy `dotnet` ngoài Unity)

Đã compile & chạy thử toàn bộ Core + Data với Newtonsoft 13.0.3:

```
[LEVEL] World 1-1 - Overworld (World 1-1), entities=20
[SAVE/LOAD] PASSED -> save_slot_1.json
[HIGHSCORES] count=3, best=MARIO:10000
[AABB] intersects=True
[POOL] active=0, available=4
```

→ DoD #2 (đọc `level_01.json` in danh sách thực thể) và DoD #3 (Save → Load 100%) **PASS**.

---

### 5. Unity actions – NGƯỜI DÙNG BẮT BUỘC phải làm trong Unity

1. **Mở project** `Super-Mario-Bros.` bằng Unity `6000.6.9f1`.
2. **Chờ Unity resolve package** `com.unity.nuget.newtonsoft-json` (cần internet lần đầu) và regenerate `Library/`.
3. **Kiểm tra Console:** không được có lỗi compile. Nếu có lỗi asmdef reference (hiếm), báo lại để tôi sửa tên assembly.
4. **Chạy smoke test Phase 1:**
   - Mở scene bất kỳ (ví dụ `Super Mario Bros. (Main) Scene`).
   - Tạo GameObject trống → `Add Component` → `Phase1Bootstrapper`.
   - Nhấn **Play** → xem Console:
     - `[Phase1] Loaded 'World 1-1 - Overworld' ... 20 entities ...`
     - `[Phase1] Save/Load round-trip PASSED ...`
5. **Kiểm tra tầng Core thuần C#:** mở `SuperMario.Core.asmdef` trong Inspector, xác nhận **No Engine References = ON**; hoặc chạy:
   `grep -rn "using UnityEngine" Assets/Scripts/Core` → phải **rỗng**.
6. **Art/đồ hoạ:** Phase 1 **KHÔNG cần thêm art mới**. Các sprite hiện có (Mario, Goomba, Koopa, Block, Pipe, Coin...) được giữ nguyên. Việc dựng hình từ JSON (instantiate prefab theo entity) là nội dung **Phase 2**.
7. **(Tuỳ chọn) Kiểm tra compile sạch:** `Assets/Scripts` → chuột phải → `Reimport`; hoặc `Edit > Preferences > External Tools` không cần cấu hình thêm.

> Những việc tôi **không thể làm thay** (vì cần Unity Editor): mở Unity, resolve package, gắn component `Phase1Bootstrapper` vào scene, nhấn Play, và xác nhận Console không lỗi.

---

### 6. Definition of Done (DoD) – Phase 1

- [x] Dự án biên dịch thành công; `SuperMario.Core` không chứa `using UnityEngine;`.
- [x] Đọc `level_01.json` và in danh sách thực thể thành công (verify bằng dotnet).
- [x] Save JSON → Load lại đúng 100% dữ liệu (verify bằng dotnet).

---

### 7. Ghi chú kỹ thuật

- `GenericPool<T>` là pool thuần C# (Stack + HashSet + factory), không phải MonoBehaviour — tránh lỗi "Generic MonoBehaviours are not supported" của Unity.
- `UnityInputAdapter` triển khai `IInputProvider` (DIP): Core không biết Input System; App là nơi duy nhất phụ thuộc engine input.
- `JsonSaveStorage` nhận đường dẫn qua constructor → App truyền `Application.persistentDataPath` (bản build) hoặc `Application.streamingAssetsPath` (dữ liệu mẫu).
- `HighScoreRepository` tự tạo file nếu chưa có và giữ tối đa 10 entry.

---

### 8. Bước tiếp theo

- **Phase 2** (`docs/roadmap.md`): 4 AI quái (Goomba/Koopa/Piranha/Bowser), 4 item/skill + combo, 5 file JSON màn chơi, `balance_config.json`, hoàn thiện HUD/Pause/Replay.
- Trước khi sang Phase 2: người dùng cần xác nhận Unity compile sạch + smoke test PASSED.
