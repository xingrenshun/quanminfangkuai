import re, json

path = r'D:\Unity\QuanMingFangKuai\Assets\Blocks\Scenes\Main.unity'
with open(path, 'r', encoding='utf-8') as f:
    content = f.read()

# Find all quoted strings that contain literal \uXXXX escapes
strings = re.findall(r'"[^"]*(?:' + r'\\u[0-9a-fA-F]{4}' + r'[^"]*)+"', content)
chinese_strings = []
for s in strings:
    try:
        decoded = json.loads(s)
    except Exception as e:
        continue
    if any('\u4e00' <= c <= '\u9fff' for c in decoded):
        chinese_strings.append(decoded)

print(f"Found {len(set(chinese_strings))} unique Chinese strings in Main.unity:")
for s in sorted(set(chinese_strings)):
    print(repr(s))
