# 🚀 TurboResX

<p align="center">
  <img src="icon.png" width="180" />
</p>

<p align="center">
  <b>Fast modern RESX localization tool for WinForms & VB.NET projects.</b>
</p>

<p align="center">
  Automatically translate `.resx` files, generate localization resources, and update `.vbproj` files in seconds.
</p>

---

## ✨ Features

* ⚡ Ultra-fast `.resx` translation
* 🌍 Multi-language localization
* 🧠 Smart UI-text detection
* 📁 Folder & single-file support
* 🔄 Translate existing language resources
* 🔐 API-key protected backend
* 🧩 Automatic `.vbproj` integration
* 🖥 Modern terminal interface
* 🚀 Async processing
* 🧼 Clean UTF-8 output
* 🔧 Private PHP translation API support
* 💡 WinForms localization automation

---

# 📸 Preview

```txt
 ___________          ___.         __________              ____  ___
 \__    ___/_ ________\_ |__   ____\______   \ ____   _____\   \/  /
   |    | |  |  \_  __ \ __ \ /  _ \|       _// __ \ /  ___/\     /
   |    | |  |  /|  | \/ \_\ (  <_> )    |   \  ___/ \___ \ /     \
   |____| |____/ |__|  |___  /\____/|____|_  /\___  >____  >___/\  \
                           \/              \/     \/     \/      \_/

             TurboResX Translator
```

---

# 🧠 Smart Translation Engine

TurboResX only translates UI-related values.

### ✅ Supported

```xml
<data name="Button1.Text">
    <value>Clean</value>
</data>

<data name="Column1.HeaderText">
    <value>Name</value>
</data>
```

### ❌ Ignored

```xml
.Size
.Location
.Type
.ZOrder
metadata
assembly
```

---

# 🌍 Localization Workflow

### Main Translation

```txt
Form1.resx
    ↓
Form1.ar.resx
```

### Chain Translation

```txt
Form1.ar.resx
    ↓
Form1.fr.resx
```

---

# 📦 Auto Project Integration

TurboResX can automatically inject generated localization files into your `.vbproj`.

### Example

```xml
<EmbeddedResource Include="Form1.de.resx">
  <DependentUpon>Form1.vb</DependentUpon>
</EmbeddedResource>
```

---

# ⚙️ Requirements

* Windows
* .NET 6 / .NET 8
* VB.NET WinForms project
* PHP translation API

---

# 🔐 API Authentication

TurboResX supports private API-key protected translation servers.

### Change Key Command

```txt
change key
```

API keys are automatically saved locally.

---

# 🖥 Usage

### Translate Main Resources

```txt
Source language: en
Target language: ar
```

### Translate Existing Localization

```txt
Source language: ar
Target language: fr
```

---

# 📂 Project Structure

```txt
TurboResX/
│
├── Program.vb
├── ResxTranslator.vb
├── LibreTranslate.vb
├── ProjectUpdater.vb
├── config.ini
└── README.md
```

---

# 🚀 PHP Translation Backend

TurboResX supports a private PHP backend that:

* Receives `.resx`
* Parses XML
* Translates UI text
* Returns translated `.resx`
* Supports API keys
* Uses Google Translate backend

---

# 🛣 Roadmap

* [ ] GUI version
* [ ] Batch translation queue
* [ ] Translation cache
* [ ] Retry system
* [ ] Multi-thread optimization
* [ ] AI translation providers
* [ ] Translation memory
* [ ] Dark desktop UI

---

# 📄 License

MIT License

---

# ❤️ Author

Made with passion by **Miro**
