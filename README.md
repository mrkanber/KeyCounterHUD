<div align="center">

# ⌨️ KeyCounterHUD

**A tiny, free keystroke counter overlay for Windows.**

Sits quietly in the corner of your screen and counts every key you press — no account, no telemetry, no catch.

[![Download](https://img.shields.io/github/v/release/mrkanber/KeyCounterHUD?label=Download&style=for-the-badge)](https://github.com/mrkanber/KeyCounterHUD/releases/latest)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg?style=for-the-badge)](LICENSE)
![.NET 10](https://img.shields.io/badge/.NET-10-512BD4?style=for-the-badge)
![Windows](https://img.shields.io/badge/Windows-11-0078D6?style=for-the-badge)

</div>

---

## Why this exists

A keystroke counter is a five-minute idea, not a product. This one is built end-to-end with [Claude Code](https://claude.com/claude-code) and released free and open source instead of sitting behind a "Pro" tier somewhere.

## Features

- **Borderless HUD** — just the number, floating on the desktop, no window chrome
- **Drag anywhere** — left-click and drag the number to reposition it
- **Persistent total** — your count survives restarts, saved to a small config file
- **Color picker** — right-click → **Renk** to pick from a few preset text colors
- **Launch at startup** — optional, toggleable from the right-click menu or tray icon
- **Tray icon** — quick access to reset/exit even if the HUD is dragged off-screen

## Download

Grab the latest build from the [**Releases**](https://github.com/mrkanber/KeyCounterHUD/releases/latest) page — no installer, just run the executable.

## Build from source

Requires [.NET 10 SDK](https://dotnet.microsoft.com/download) on Windows.

```
git clone git@github.com:mrkanber/KeyCounterHUD.git
cd KeyCounterHUD
dotnet build
dotnet run
```

## Configuration

Settings live at `%AppData%\KeyCounterHUD\config.json` — total count, position, and text color.

## Quitting

Right-click the counter (or the tray icon) → **Çıkış**.

## Contributing

Issues and PRs are welcome. This is a small hobby project, so keep changes focused and simple.

## License

[MIT](LICENSE)
