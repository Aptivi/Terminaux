import os


def vnd_increment(old_version, new_version, api_versions):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))

    # Replace the version in the Directory.Build.props file
    props_file = f"{solution}/Directory.Build.props"
    props_line_idx = 0
    props_lines = []
    with open(props_file) as props_stream:
        props_lines = props_stream.readlines()
        version_found = False
        for line in props_lines:
            props_line_idx += 1
            if '<Version>' in line and '</Version>' in line:
                substr_idx = line.index('<') + 9
                substr_idx_end = line[substr_idx:].index('<') + substr_idx
                props_version = line[substr_idx:substr_idx_end]
                version_found = old_version in props_version
                break
        if not version_found:
            raise Exception(f"Version {old_version} not found in "
                            "Directory.Build.props.")
    props_lines[props_line_idx - 1] = props_lines[props_line_idx - 1] \
                                  .replace(old_version,
                                           new_version)
    with open(props_file, "w") as props_stream:
        props_stream.writelines(props_lines)

    # Replace the version in the CHANGES.TITLE file
    changes_title_file = f"{solution}/CHANGES.TITLE"
    changes_title_lines = []
    with open(changes_title_file) as changes_title_stream:
        changes_title_lines = changes_title_stream.readlines()
    changes_title_lines[0] = changes_title_lines[0].replace(old_version,
                                                            new_version)
    with open(changes_title_file, 'w') as changes_title_stream:
        changes_title_stream.writelines(changes_title_lines)