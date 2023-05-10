# Intro
Send requests to check if the configured APIs are up.

# How to use
1. Write the configuration files using the example bellow.
2. Set the `const CONFIG_FILES_PATH` in  `MainForm` class with the absolute path where the configuration files are stored.
3. Set the `const INTERVAL_IN_SECONDS` in `MainForm`.
4. Run.

# Config file example
Defined in the class `RequestConfig`.
```json
{
	"Id": "some identifier",
	"Uri": "https://myurl.dev/some/some?query=value",
	"Method": "GET",
	"Auth": {
		"Username": "usr",
		"Password": "pwd"
	},
	"Headers": {
		"someheader": "abc",
		"someotherheader": "def"
	}
}
```