ROOT_DIR := $(shell dirname "$(realpath $(lastword $(MAKEFILE_LIST)))")
OUTPUTS  := \
	-name "bin" -or \
	-name "obj" -or \
	-name "docs"

.PHONY: all

# General use

all: all-online

all-online: invoke-build

dbg:
	$(MAKE) invoke-build ENVIRONMENT=Debug

dbg-ci:
	$(MAKE) invoke-build-ci ENVIRONMENT=Debug

rel-ci:
	$(MAKE) invoke-build-ci ENVIRONMENT=Release

doc: invoke-doc-build

clean:
	find "$(ROOT_DIR)" -type d \( $(OUTPUTS) \) -print -exec rm -rf "{}" +

# Below targets specify functions for full build

invoke-build:
	bash tools/build.sh "$(ENVIRONMENT)"

invoke-build-ci:
	bash tools/build.sh "$(ENVIRONMENT)" -p:ContinuousIntegrationBuild=true

invoke-doc-build:
	bash tools/docgen.sh
