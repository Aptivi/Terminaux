# Below is a workaround for .NET SDK 7.0 trying to allocate large amounts of memory for GC work:
# https://github.com/dotnet/runtime/issues/85556#issuecomment-1529177092
DOTNET_HEAP_LIMIT_INT = $(shell sysctl -n hw.memsize 2>/dev/null || grep MemAvailable /proc/meminfo | awk '{print $$2 * 1024}')
DOTNET_HEAP_LIMIT = $(shell printf '%X\n' $(DOTNET_HEAP_LIMIT_INT))

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
	./tools/build.sh "$(ENVIRONMENT)" || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./tools/build.sh "$(ENVIRONMENT)")

invoke-build-ci:
	./tools/build.sh "$(ENVIRONMENT)" -p:ContinuousIntegrationBuild=true || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./tools/build.sh "$(ENVIRONMENT)" -p:ContinuousIntegrationBuild=true)

invoke-doc-build:
	./tools/docgen.sh || (echo Retrying with heap limit 0x$(DOTNET_HEAP_LIMIT)... && DOTNET_GCHeapHardLimit=$(DOTNET_HEAP_LIMIT) ./tools/docgen.sh)
