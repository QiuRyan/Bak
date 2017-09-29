/*!
 * jym Gruntfile
 * https://jym-government-api.jinyinmao.com.cn
 */

module.exports = function(grunt) {
    'use strict';

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),

        'azure-blob-upload': {
            release: {
                options: {
                    serviceOptions: 'BlobEndpoint=https://dev.blob.core.chinacloudapi.cn/;QueueEndpoint=https://dev.queue.core.chinacloudapi.cn/;TableEndpoint=https://dev.table.core.chinacloudapi.cn/;AccountName=dev;AccountKey=HXNxBHrfjdujQWB/zfmBcAb8wvqP4qqgvfem9wJL3Df1UnZBxM+iYjKcwPox9pA4vHTbm0clHkdZxqdDBMu5pw==',
                    container: 'releases',
                    blobProperties: 'contentType: "application/zip"'
                },
                src: 'Jinyinmao.<%= pkg.name %>@<%= pkg.version %>.zip',
                dest: 'Jinyinmao.<%= pkg.name %>/Jinyinmao.<%= pkg.name %>@<%= pkg.version %>.zip'
            }
        },

        bump: {
            options: {
                files: ['package.json'],
                updateConfigs: ['pkg'],
                commit: false,
                commitMessage: '%VERSION%',
                commitFiles: ['package.json'],
                createTag: false,
                tagName: 'v%VERSION%',
                tagMessage: 'Version %VERSION%',
                push: false,
                pushTo: 'upstream',
                gitDescribeOptions: '--tags --always --abbrev=1 --dirty=-d',
                globalReplace: false,
                prereleaseName: false,
                regExp: false
            }
        },

        clean: {
            options: {
                force: true
            },
            obj: {
                src: ['../Source/**/obj/**/*']
            },
            release: {
                src: ['../../**/*'],
                dot: true
            },
            npm: 'node_modules/**/*'
        },

        copy: {
            options: {
                timestamp: true
            },
            readme: {
                src: '../README.md',
                dest: '../Release/README.md'
            },
            release: {
                expand: true,
                cwd: '../Publish/',
                src: ['**/*'],
                dest: '../Release/'
            }
        },

        compress: {
            release: {
                options: {
                    archive: 'Jinyinmao.<%= pkg.name %>@<%= pkg.version %>.zip'
                },
                expand: true,
                cwd: '../Release/',
                src: ['**/*'],
                dest: '.'
            }
        },

        exec: {
            options: {
                stdout: true,
                stderr: true
            },
            gitCommit: {
                command: [
                    'git add --all',
                    'git commit -a -m "Jinyinmao.<%= pkg.name %>@<%= pkg.version %>"'
                ].join('&&')
            },
            gitPush: {
                command: [
                    'git tag -a Jinyinmao.<%= pkg.name %>@<%= pkg.version %> -m "Jinyinmao.<%= pkg.name %>@<%= pkg.version %>" -f',
                    'git push origin <%= pkg.name %>-Release Jinyinmao.<%= pkg.name %>@<%= pkg.version %> -f'
                ].join('&&')
            },
            npmInstall: {
                command: 'npm install'
            },
            npmUpdate: {
                command: 'npm update --save-dev'
            }
        },

        replace: {
            version: {
                src: ['../Source/**/AssemblyInfo.cs'],
                overwrite: true,
                replacements: [
                    {
                        from: /[assembly: AssemblyVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)]/g,
                        to: '[assembly: AssemblyVersion("<%= pkg.version %>.*")]'
                    }, {
                        from: /[assembly: AssemblyFileVersion\("[0-9]+(\.([0-9]+|\*)){1,3}"\)]$/,
                        to: '[assembly: AssemblyFileVersion("<%= pkg.version %>.0")]'
                    }
                ]
            },
            apiSize: {
                src: ['../Source/Jinyinmao.Government.CloudService/ServiceDefinition.csdef'],
                overwrite: true,
                replacements: [
                    {
                        from: '<WebRole name="Jinyinmao.Government" vmsize="Small">',
                        to: '<WebRole name="Jinyinmao.Government" vmsize="Large">'
                    }
                ]
            }
        }
    });

    // This command registers the default task which will install bower packages into wwwroot/lib
    grunt.registerTask('default', ['exec:npmUpdate']);

    grunt.registerTask('version', ['bump', 'replace:version', 'replace:version']); // execute twice
    grunt.registerTask('gitCommit', 'exec:gitCommit');
    grunt.registerTask('gitPush', 'exec:gitPush');

    grunt.registerTask('upgradeCloudServiceSize', ['replace:apiSize']);

    grunt.registerTask('release', ['copy:readme', 'copy:release', 'compress:release', 'azure-blob-upload:release', 'clean:release']);

    // These plugins provide necessary tasks.
    require('load-grunt-tasks')(grunt, {
        scope: 'devDependencies'
    });
    require('time-grunt')(grunt);
};