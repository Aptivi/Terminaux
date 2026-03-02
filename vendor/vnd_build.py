import os
import subprocess


def vnd_build(args):
    solution = os.path.dirname(os.path.abspath(__file__ + '/../'))
    solution = solution + "/Terminaux.slnx"
    command = f"dotnet build {solution} {args}"
    result = subprocess.run(command, shell=True)
    if result.returncode != 0:
        raise Exception("Build failed with code %i" % (result.returncode))
